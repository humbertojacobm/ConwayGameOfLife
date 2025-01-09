using AutoMapper;
using ConwayGameOfLife.Common;
using ConwayGameOfLife.DatabaseModels;
using ConwayGameOfLife.DTO;
using ConwayGameOfLife.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Core
{
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly IBoardStateRepository _boardStateRepository;
        private readonly IMapper _mapper;

        public GameOfLifeService(
            IBoardStateRepository boardStateRepository,
            IMapper mapper)
        {
            _boardStateRepository = boardStateRepository;
            _mapper = mapper;
        }

        public Guid UploadBoardState(BoardDTO boardDto)
        {
            var board = _mapper.Map<Board>(boardDto);
            board.Step = 0;
            board.Id = Guid.NewGuid();

            _boardStateRepository.CreateBoard(board);

            return board.Id;
        }

        public BoardDTO GetNextState(Guid boardId)
        {
            var board = _boardStateRepository.GetBoard(boardId);
            if (board == null)
                throw new ArgumentException("Board not found", nameof(boardId));

            var nextBoard = ApplyConwayRules(board);

            board.Step++;

            nextBoard.Step = board.Step;

            _boardStateRepository.SaveBoard(board);

            return _mapper.Map<BoardDTO>(nextBoard);
        }

        public BoardDTO GetXStepsState(Guid boardId, int requestedStep)
        {
            var board = _boardStateRepository.GetBoard(boardId);
            if (board == null)
                throw new ArgumentException("Board not found", nameof(boardId));

            if (requestedStep < board.Step)
            {
                throw new InvalidOperationException(
                    $"Requested step {requestedStep} is less than the current step {board.Step}. Backward stepping is not allowed.");
            }

            int stepsToAdvance = requestedStep - board.Step;

            for (int i = 0; i < stepsToAdvance; i++)
            {
                board = ApplyConwayRules(board);
                board.Step++;
            }

            _boardStateRepository.SaveBoard(board);

            return _mapper.Map<BoardDTO>(board);
        }

        public BoardDTO GetFinalState(Guid boardId, int maxAttempts)
        {
            var currentBoard = _boardStateRepository.GetBoard(boardId);
            if (currentBoard == null)
                throw new ArgumentException("Board not found.", nameof(boardId));

            for (int i = 0; i < maxAttempts; i++)
            {
                var nextBoard = ApplyConwayRules(currentBoard);

                if (Helper.AreBoardsEqual(currentBoard, nextBoard))
                {
                    _boardStateRepository.SaveBoard(currentBoard);

                    return _mapper.Map<BoardDTO>(currentBoard);
                }

                nextBoard.Step = currentBoard.Step + 1;
                _boardStateRepository.SaveBoard(nextBoard);
                currentBoard = nextBoard;
            }

            throw new InvalidOperationException(
                $"Board does not reach a stable (final) state after {maxAttempts} attempts.");
        }

        private Board ApplyConwayRules(Board board)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            var updatedCells = new bool[board.Height, board.Width];

            for (int row = 0; row < board.Height; row++)
            {
                for (int col = 0; col < board.Width; col++)
                {
                    int livingNeighbors = CountLivingNeighbors(board, row, col);
                    bool isAlive = board.Cells[row, col];

                    updatedCells[row, col] = isAlive
                        ? (livingNeighbors == 2 || livingNeighbors == 3)
                        : (livingNeighbors == 3);
                }
            }

            board.Cells = updatedCells;

            return board;
        }

        private int CountLivingNeighbors(Board board, int row, int col)
        {
            int count = 0;

            for (int nr = row - 1; nr <= row + 1; nr++)
            {
                for (int nc = col - 1; nc <= col + 1; nc++)
                {
                    // Skip the cell itself
                    if (nr == row && nc == col)
                        continue;

                    // Check boundaries
                    if (nr >= 0 && nr < board.Height && nc >= 0 && nc < board.Width)
                    {
                        if (board.Cells[nr, nc])
                            count++;
                    }
                }
            }

            return count;
        }
    }
}
