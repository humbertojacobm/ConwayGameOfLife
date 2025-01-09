using ConwayGameOfLife.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Infrastructure.Repository
{
    public class BoardStateRepository : IBoardStateRepository
    {
        private readonly AppDbContext _context;

        public BoardStateRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateBoard(Board board)
        {
            _context.Boards.Add(board);
            _context.SaveChanges();
        }

        public void SaveBoard(Board board)
        {
            _context.Boards.Update(board);
            _context.SaveChanges();
        }

        public Board? GetBoard(Guid boardId)
        {
            return _context.Boards.Find(boardId);
        }

        public Board? GetBoardAsNotTracked(Guid boardId)
        {
            return _context.Boards
                .AsNoTracking()
                .FirstOrDefault(b => b.Id == boardId);
        }
    }
}
