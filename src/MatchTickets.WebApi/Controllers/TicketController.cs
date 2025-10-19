using MatchTickets.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatchTickets.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("client/{clientId}")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<IActionResult> GetByClient(int clientId)
        {
            var tickets = await _ticketService.GetTicketsByClientAsync(clientId);
            return Ok(tickets);
        }

        [HttpPost("buy")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<IActionResult> BuyTicket([FromBody] int matchId)
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var ticket = await _ticketService.CreateTicketAsync(clientId, matchId);
            return Ok(ticket);
        }
    }

}
