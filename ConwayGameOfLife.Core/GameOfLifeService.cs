using AutoMapper;
using ConwayGameOfLife.Common;
using ConwayGameOfLife.EntityLayer;
using ConwayGameOfLife.DTO;
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

        public async Task<Guid> UploadBoardStateAsync(BoardDTO boardDto)
        {
            var board = _mapper.Map<Board>(boardDto);
            board.Step = 0;
            board.Id = Guid.NewGuid();

            await _boardStateRepository.CreateBoardAsync(board);

            return board.Id;
        }

        public async Task<BoardDTO> GetNextStateAsync(Guid boardId)
        {
            var board = await _boardStateRepository.GetBoardAsync(boardId);
            if (board == null)
                throw new ArgumentException("Board not found", nameof(boardId));

            var nextBoard = ApplyConwayRules(board);

            if (Helper.AreBoardsEqual(board, nextBoard))
            {
                nextBoard.Step = board.Step;
            }
            else
            {
                board.Cells = nextBoard.Cells;
                board.Step++;
                nextBoard.Step = board.Step;

                await _boardStateRepository.SaveBoardAsync(board);
            }

            return _mapper.Map<BoardDTO>(nextBoard);
        }

        public async Task<BoardDTO> GetXStepsStateAsync(Guid boardId, int requestedStep)
        {
            var board = await _boardStateRepository.GetBoardAsNotTrackedAsync(boardId);
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
                var nextBoard = ApplyConwayRules(board);

                if (Helper.AreBoardsEqual(board, nextBoard))
                {
                    nextBoard.Step = board.Step;
                    board = nextBoard;
                    break;
                }
                else
                {
                    board.Cells = nextBoard.Cells;
                    board.Step++;
                    nextBoard.Step = board.Step;
                    board = nextBoard;
                }
            }

            var finalBoard = await _boardStateRepository.GetBoardAsync(boardId);
            if (finalBoard == null)
                throw new InvalidOperationException("Board record not found in DB while updating final state.");

            finalBoard.Step = board.Step;
            finalBoard.Cells = board.Cells;

            await _boardStateRepository.SaveBoardAsync(finalBoard);

            return _mapper.Map<BoardDTO>(board);
        }

        public async Task<BoardDTO> GetFinalStateAsync(Guid boardId, int maxAttempts)
        {
            var board = await _boardStateRepository.GetBoardAsNotTrackedAsync(boardId);
            if (board == null)
                throw new ArgumentException("Board not found.", nameof(boardId));

            bool isSteady = false;

            for (int i = 0; i < maxAttempts; i++)
            {
                var nextBoard = ApplyConwayRules(board);

                if (Helper.AreBoardsEqual(board, nextBoard))
                {
                    nextBoard.Step = board.Step;
                    board = nextBoard;
                    isSteady = true;
                    break;
                }
                else
                {
                    board.Cells = nextBoard.Cells;
                    board.Step++;
                    nextBoard.Step = board.Step;
                    board = nextBoard;
                }
            }

            if (!isSteady)
            {
                throw new InvalidOperationException(
                    $"Board does not reach a stable (final) state after {maxAttempts} attempts.");
            }

            var finalBoard = await _boardStateRepository.GetBoardAsync(boardId);
            if (finalBoard == null)
                throw new InvalidOperationException("Board record not found in DB while updating final state.");

            finalBoard.Step = board.Step;
            finalBoard.Cells = board.Cells;
            await _boardStateRepository.SaveBoardAsync(finalBoard);

            return _mapper.Map<BoardDTO>(board);
        }

        private Board ApplyConwayRules(Board currentBoard)
        {
            if (currentBoard == null)
                throw new ArgumentNullException(nameof(currentBoard));

            var nextBoard = new Board
            {
                Id = currentBoard.Id,
                Width = currentBoard.Width,
                Height = currentBoard.Height,
                Cells = new bool[currentBoard.Height, currentBoard.Width],
                Step = currentBoard.Step
            };

            for (int row = 0; row < currentBoard.Height; row++)
            {
                for (int col = 0; col < currentBoard.Width; col++)
                {
                    int livingNeighbors = CountLivingNeighbors(currentBoard, row, col);
                    bool isAlive = currentBoard.Cells[row, col];

                    nextBoard.Cells[row, col] = isAlive
                        ? (livingNeighbors == 2 || livingNeighbors == 3)
                        : (livingNeighbors == 3);                        
                }
            }

            return nextBoard;
        }

        private int CountLivingNeighbors(Board board, int row, int col)
        {
            int count = 0;

            for (int nr = row - 1; nr <= row + 1; nr++)
            {
                for (int nc = col - 1; nc <= col + 1; nc++)
                {
                    if (nr == row && nc == col)
                        continue;

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
