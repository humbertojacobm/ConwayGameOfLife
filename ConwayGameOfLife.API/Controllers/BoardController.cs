using ConwayGameOfLife.Core;
using ConwayGameOfLife.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConwayGameOfLife.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IGameOfLifeService _gameOfLifeService;

        public BoardController(IGameOfLifeService gameOfLifeService)
        {
            _gameOfLifeService = gameOfLifeService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadBoardState([FromBody] BoardDTO boardDto)
        {
            var newBoardId = await _gameOfLifeService.UploadBoardStateAsync(boardDto);
            return Ok(new { BoardId = newBoardId });
        }

        [HttpGet("{id}/next")]
        public async Task<IActionResult> GetNextState(Guid id)
        {
            var nextBoardState = await _gameOfLifeService.GetNextStateAsync(id);
            return Ok(nextBoardState);
        }

        [HttpGet("{id}/steps/{x}")]
        public async Task<IActionResult> GetXSteps(Guid id, int x)
        {
            if (x < 0)
                return BadRequest("Number of steps must be non-negative.");

            try
            {
                var boardDto = await _gameOfLifeService.GetXStepsStateAsync(id, x);
                return Ok(boardDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/final/{maxAttempts}")]
        public async Task<IActionResult> GetFinalState(Guid id, int maxAttempts)
        {
            if (maxAttempts < 1)
                return BadRequest("Max attempts must be greater than 0.");

            try
            {
                var finalBoardDto = await _gameOfLifeService.GetFinalStateAsync(id, maxAttempts);
                return Ok(finalBoardDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
