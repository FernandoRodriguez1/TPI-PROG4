using MatchTickets.Application.DTOs;
using MatchTickets.Application.Exceptions;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatchTickets.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipCardService _membershipService;

        public MembershipController(IMembershipCardService membershipService)
        {
            _membershipService = membershipService;
        }


        [HttpPost("join")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<IActionResult> CreateMembershipCard([FromBody] JoinClubRequest request)
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var response = await _membershipService.CreateMembershipAsync(
                clientId,
                request.ClubId,
                request.Plan
            );

            return Ok(response);
        }


        [HttpGet("byclient")]
        [Authorize(Policy = "BothPolicy")]
        public async Task<IActionResult> GetMembershipByClientIDAsync()
        {
            var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var card = await _membershipService.GetMembershipCardByClientIdAsync(clientId);

            return Ok(new
            {
                membershipId = card.MembershipId,
                cardNumber = card.MembershipCardNumber,
                clubId = card.ClubId,
                plan = card.Plan.ToString(),
                dischargeDate = card.DischargeDate,
                expirationDate = card.ExpirationDate
            });
        }

    }
}
