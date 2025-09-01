using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MatchTickets.WebApi.Controllers;

[Route("/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
   private readonly IUserService _userService;

    public UserController(IUserService userService)
    {  
        _userService = userService;
    }

    [HttpGet("get-user-by-email")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        var user = await _userService.GetUserByEmailAsync(new Email(email));
        if (user is null)
            return NotFound();

        return Ok(user);
    }
    [HttpPost("add-user")]
    public async Task<IActionResult> AddClient([FromBody] ClientDTO clientDto)
    {
        await _userService.AddClientAsync(clientDto);
        return Ok("Client created successfully");
    }

}

