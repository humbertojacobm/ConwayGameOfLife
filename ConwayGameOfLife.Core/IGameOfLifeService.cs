using ConwayGameOfLife.DTO;

namespace ConwayGameOfLife.Core
{
    public interface IGameOfLifeService
    {
        Guid UploadBoardState(BoardDTO boardDto);
        BoardDTO GetNextState(Guid boardId);
        //BoardDTO GetXStepsState(Guid boardId, int steps);
        //BoardDTO? GetFinalState(Guid boardId);
    }
}
