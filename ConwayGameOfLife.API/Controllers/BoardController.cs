﻿using ConwayGameOfLife.Core;
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

        //[HttpGet("{id}/final")]
        //public IActionResult GetFinalState(Guid id)
        //{
        //    var finalState = _gameOfLifeService.GetFinalState(id);
        //    if (finalState == null)
        //    {
        //        return BadRequest("Board does not reach a final state in the specified attempt limit.");
        //    }

        //    return Ok(finalState);
        //}
    }
}
