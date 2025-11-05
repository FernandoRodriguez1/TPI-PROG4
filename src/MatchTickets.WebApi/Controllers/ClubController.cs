using MatchTickets.Application.DTOs;
using MatchTickets.Application.Exceptions;
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
            var club = await _clubService.GetByIdAsync(id);
            return Ok(club);
        }

        
        [HttpGet("{id}/matches")]
        [Authorize(Policy = "BothPolicy")]
        public async Task<IActionResult> GetMatches(int id)
        {
            var matches = await _clubService.GetMatchesAsync(id);
            return Ok(matches);
        }

        
        [HttpGet("{id}/members/count")]
        [Authorize(Policy = "BothPolicy")]
        public async Task<IActionResult> GetMembersCount(int id)
        {
            var count = await _clubService.GetMembersCountAsync(id);
            return Ok(new { clubId = id, membersCount = count });
        }


        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Update([FromBody] ClubDTO clubDto)
        {
            await _clubService.UpdateAsync(clubDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clubService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] ClubDTO clubDto)
        {
            await _clubService.AddAsync(clubDto);
            clubDto.ClubId = clubDto.ClubId;
            return CreatedAtAction(nameof(GetById), new { id = clubDto.ClubId }, clubDto);
        }
    }
}
