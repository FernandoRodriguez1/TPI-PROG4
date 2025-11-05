using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchTickets.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoccerMatchController : ControllerBase
    {
        private readonly ISoccerMatchService _soccerMatchService;

        public SoccerMatchController(ISoccerMatchService soccerMatchService)
        {
            _soccerMatchService = soccerMatchService;
        }

        [HttpGet]
        [Authorize(Policy = "BothPolicy")]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _soccerMatchService.GetAllMatchesAsync();
            return Ok(matches);
        }

        
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            var match = await _soccerMatchService.GetMatchByIdAsync(id);
            return Ok(match);
        }

        
        [HttpGet("club/{clubId}")]
        [Authorize(Policy = "BothPolicy")]
        public async Task<IActionResult> GetMatchesByClub(int clubId)
        {
            var matches = await _soccerMatchService.GetMatchesByClubAsync(clubId);
            return Ok(matches);
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddMatch([FromBody] SoccerMatchDTO matchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _soccerMatchService.AddMatchAsync(matchDto);

                // CreatedAtAction devuelve el recurso creado
                return CreatedAtAction(nameof(GetMatchById), new { id = matchDto.SoccerMatchId }, matchDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateMatch( [FromBody] SoccerMatchDTO matchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            await _soccerMatchService.UpdateMatchAsync(matchDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            await _soccerMatchService.DeleteMatchAsync(id);

            return NoContent();
        }
    }

}
