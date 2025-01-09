using ConwayGameOfLife.DatabaseModels;

namespace ConwayGameOfLife.Infrastructure.Repository
{
    public interface IBoardStateRepository
    {
        void CreateBoard(Board board);
        void SaveBoard(Board board);
        Board? GetBoard(Guid boardId);
    }
}
