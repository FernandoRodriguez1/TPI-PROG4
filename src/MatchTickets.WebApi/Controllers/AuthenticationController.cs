using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MatchTickets.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticateService _authenticationService;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthenticationController(IAuthenticateService authenticationService, IUserService userService, IConfiguration configuration)
    {
        _authenticationService = authenticationService;
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Authenticate([FromBody] AuthenticationRequestBody authenticationRequestBody)
    {
        var validateUserResult = _authenticationService.ValidateUser(authenticationRequestBody);

        if (validateUserResult.Message == "wrong username")
            return NotFound("User not found.");

        if (validateUserResult.Message == "wrong password")
            return Unauthorized("Invalid password.");

        if (!validateUserResult.Result)
            return BadRequest("Authentication failed.");

        
        var user = _userService.GetUserByEmail(authenticationRequestBody.Email);

        // generar clave y credenciales
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // claims
        var claimsForToken = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim("username", user.UserName),
            new Claim(ClaimTypes.Role, user.UserType.ToString())

        };

        // crear token
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claimsForToken,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return Ok(new { token = $"Bearer {tokenToReturn}" });

    }
}

