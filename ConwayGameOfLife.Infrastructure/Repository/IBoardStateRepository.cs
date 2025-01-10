using ConwayGameOfLife.DatabaseModels;

namespace ConwayGameOfLife.Infrastructure.Repository
{
    public interface IBoardStateRepository
    {
        Task CreateBoardAsync(Board board);
        Task SaveBoardAsync(Board board);
        Task<Board?> GetBoardAsync(Guid boardId);
        Task<Board?> GetBoardAsNotTrackedAsync(Guid boardId);
    }
}
