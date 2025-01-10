using ConwayGameOfLife.DTO;

namespace ConwayGameOfLife.Core
{
    public interface IGameOfLifeService
    {
        Task<Guid> UploadBoardStateAsync(BoardDTO boardDto);
        Task<BoardDTO> GetNextStateAsync(Guid boardId);
        Task<BoardDTO> GetXStepsStateAsync(Guid boardId, int requestedStep);
        Task<BoardDTO> GetFinalStateAsync(Guid boardId, int maxAttempts);
    }
}
