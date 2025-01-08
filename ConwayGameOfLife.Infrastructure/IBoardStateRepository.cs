using ConwayGameOfLife.DatabaseModels;

namespace ConwayGameOfLife.Infrastructure
{
    public interface IBoardStateRepository
    {
        void SaveBoard(Board board);
        Board? GetBoard(Guid boardId);
    }
}
