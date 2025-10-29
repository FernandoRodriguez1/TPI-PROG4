﻿using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
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
    [HttpGet("admins")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetAdmins()
    {
        var admins = await _userService.GetAdminsAsync();
        if (!admins.Any())
            return NotFound("No admins created.");

        return Ok(admins);
    }
    [HttpGet("clients")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetClients()
    {
        var clients = await _userService.GetClientsAsync();
        if (!clients.Any())
            return NotFound("No clients created.");

        return Ok(clients);
    }
    [HttpGet("user-by-id")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetUserById([FromQuery] int id)
    {
        var user = await _userService.GetClientByIdAsync(id);
        if (user is null)
            return NotFound("User not defined");

        return Ok(user);
    }

    [HttpGet("user-by-email")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        var user = await _userService.GetClientByEmailAsync(new Email(email));
        if (user is null)
            return NotFound("Email not found");

        return Ok(user);
    }
    [HttpPost("client")]

    public async Task<IActionResult> AddClient([FromBody] ClientDTO clientDto)
    {
        await _userService.AddClientAsync(clientDto);
        return Ok("Client created successfully");
    }

    [HttpPost("admin")]
    //[Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> AddAdmin([FromBody] AdminDTO adminDto)
    {
        await _userService.AddAdminAsync(adminDto);
        return Ok("Admin created successfully");
    }

    [HttpDelete("client")]
    [Authorize(Policy = "BothPolicy")]
    public async Task<IActionResult> DeleteClient([FromQuery] int clientid)
    {
        await _userService.DeleteClientAsync(clientid);
        return Ok("Client deleted successfully");
    }
}

