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
        public IActionResult UploadBoardState([FromBody] BoardDTO boardDto)
        {
            var newBoardId = _gameOfLifeService.UploadBoardState(boardDto);
            return Ok(new { BoardId = newBoardId });
        }

        [HttpGet("{id}/next")]
        public IActionResult GetNextState(Guid id)
        {
            var nextBoardState = _gameOfLifeService.GetNextState(id);
            return Ok(nextBoardState);
        }

        [HttpGet("{id}/steps/{x}")]
        public IActionResult GetXSteps(Guid id, int x)
        {
            if (x < 0)
                return BadRequest("Number of steps must be non-negative.");

            try
            {
                var boardDto = _gameOfLifeService.GetXStepsState(id, x);
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
        public IActionResult GetFinalState(Guid id, int maxAttempts)
        {
            if (maxAttempts < 1)
                return BadRequest("Max attempts must be greater than 0.");

            try
            {
                var finalBoardDto = _gameOfLifeService.GetFinalState(id, maxAttempts);
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
