using ConwayGameOfLife.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Infrastructure
{
    public class BoardStateRepository : IBoardStateRepository
    {
        private static readonly Dictionary<Guid, Board> _boards = new();

        public void SaveBoard(Board board) => _boards[board.Id] = board;
        public Board? GetBoard(Guid boardId) =>
            _boards.TryGetValue(boardId, out var board) ? board : null;
    }
}
