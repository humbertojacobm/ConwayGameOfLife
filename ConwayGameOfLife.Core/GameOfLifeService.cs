using AutoMapper;
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
            board.Id = Guid.NewGuid();

            _boardStateRepository.SaveBoard(board);
            return board.Id;
        }

        public BoardDTO GetNextState(Guid boardId)
        {
            var board = _boardStateRepository.GetBoard(boardId);
            if (board == null)
                throw new ArgumentException("Board not found", nameof(boardId));

            var nextBoard = ApplyConwayRules(board);
            _boardStateRepository.SaveBoard(nextBoard);

            return _mapper.Map<BoardDTO>(nextBoard);
        }

        //public BoardDTO GetXStepsState(Guid boardId, int steps)
        //{
        //    // Implementation that loops or recursively calculates the states
        //    // ...
        //}

        //public BoardDTO? GetFinalState(Guid boardId)
        //{
        //    // Implementation that calculates until a stable state or max iteration is reached
        //    // Return null if it does not converge
        //}

        private Board ApplyConwayRules(Board currentBoard)
        {
            if (currentBoard == null)
                throw new ArgumentNullException(nameof(currentBoard));

            var nextBoard = new Board
            {
                Id = currentBoard.Id,
                Width = currentBoard.Width,
                Height = currentBoard.Height,
                Cells = new bool[currentBoard.Height, currentBoard.Width]
            };

            for (int row = 0; row < currentBoard.Height; row++)
            {
                for (int col = 0; col < currentBoard.Width; col++)
                {
                    int livingNeighbors = CountLivingNeighbors(currentBoard, row, col);
                    bool isAlive = currentBoard.Cells[row, col];

                    nextBoard.Cells[row, col] = isAlive
                        ? (livingNeighbors == 2 || livingNeighbors == 3) // Survive
                        : (livingNeighbors == 3);                        // Birth
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
