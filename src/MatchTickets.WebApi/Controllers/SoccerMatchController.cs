using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
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
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _soccerMatchService.GetAllMatchesAsync();
            return Ok(matches);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            var match = await _soccerMatchService.GetMatchByIdAsync(id);
            if (match == null)
                return NotFound();

            return Ok(match);
        }

        
        [HttpGet("club/{clubId}")]
        public async Task<IActionResult> GetMatchesByClub(int clubId)
        {
            var matches = await _soccerMatchService.GetMatchesByClubAsync(clubId);
            return Ok(matches);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddMatch([FromBody] SoccerMatchDTO matchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _soccerMatchService.AddMatchAsync(matchDto);
            return CreatedAtAction(nameof(GetMatchById), new { id = matchDto.SoccerMatchId }, matchDto);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch(int id, [FromBody] SoccerMatchDTO matchDto)
        {
            if (id != matchDto.SoccerMatchId)
                return BadRequest("ID mismatch");

            await _soccerMatchService.UpdateMatchAsync(matchDto);
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            await _soccerMatchService.DeleteMatchAsync(id);
            return NoContent();
        }
    }

}
