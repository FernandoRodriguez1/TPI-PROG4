using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchTickets.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        
        [HttpGet]
        [Authorize(Policy = "BothPolicy")]
        public async Task<IActionResult> GetAll()
        {
            var clubs = await _clubService.GetAllAsync();
            return Ok(clubs);
        }

       
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminPolicy")]

        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var club = await _clubService.GetByIdAsync(id);
                return Ok(club);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        
        [HttpGet("{id}/matches")]
        [Authorize(Policy = "BothPolicy")]
        public async Task<IActionResult> GetMatches(int id)
        {
            try
            {
                var matches = await _clubService.GetMatchesAsync(id);
                return Ok(matches);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        
        [HttpGet("{id}/members/count")]
        [Authorize(Policy = "BothPolicy")]
        public async Task<IActionResult> GetMembersCount(int id)
        {
            var count = await _clubService.GetMembersCountAsync(id);
            return Ok(new { clubId = id, membersCount = count });
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] ClubDTO clubDto)
        {
            if (clubDto == null)
                return BadRequest(new { error = "Club no puede ser nulo." });

            if (id != clubDto.ClubId)
                return BadRequest(new { error = "ID del club no coincide con el body." });

            try
            {
                await _clubService.UpdateAsync(clubDto);
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clubService.DeleteAsync(id);
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] ClubDTO clubDto)
        {
            if (clubDto == null)
                return BadRequest(new { error = "Club no puede ser nulo." });

            await _clubService.AddAsync(clubDto);

            // Retornar DTO con el ID generado
            clubDto.ClubId = clubDto.ClubId;
            return CreatedAtAction(nameof(GetById), new { id = clubDto.ClubId }, clubDto);
        }
    }
}
