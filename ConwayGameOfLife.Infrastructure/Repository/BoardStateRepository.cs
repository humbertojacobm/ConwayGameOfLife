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

        public async Task CreateBoardAsync(Board board)
        {
            await _context.Boards.AddAsync(board);
            await _context.SaveChangesAsync();
        }

        public async Task SaveBoardAsync(Board board)
        {
            _context.Boards.Update(board);
            await _context.SaveChangesAsync();
        }

        public async Task<Board?> GetBoardAsync(Guid boardId)
        {
            return await _context.Boards.FindAsync(boardId);
        }

        public async Task<Board?> GetBoardAsNotTrackedAsync(Guid boardId)
        {
            return await _context.Boards
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == boardId);
        }
    }
}
