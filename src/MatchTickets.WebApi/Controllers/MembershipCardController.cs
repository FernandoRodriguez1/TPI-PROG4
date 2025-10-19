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
            // Validar claim de usuario
            var subClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (subClaim == null)
                return Unauthorized(new { error = "Token inválido o claim 'sub' ausente." });

            if (request == null)
                return BadRequest(new { error = "Solicitud de membresía vacía." });

            if (request.ClubId <= 0)
                return BadRequest(new { error = "ClubId inválido." });

            var clientId = int.Parse(subClaim.Value);

            try
            {
                var card = await _membershipService.CreateMembershipAsync(clientId, request.ClubId, request.Plan);

                string message = request.ClubId switch
                {
                    1 => "¡Bienvenido Leproso! 🟥⬛",
                    2 => "¡Bienvenido Canalla! 🟡🔵",
                    3 => "¡Bienvenido Matador! 🔵⚪",
                    _ => "¡Socio creado con éxito!"
                };

                return Ok(new
                {
                    message,
                    membershipId = card.MembershipId,
                    cardNumber = card.MembershipCardNumber,
                    clubId = card.ClubId,
                    plan = card.Plan.ToString(),
                    dischargeDate = card.DischargeDate,
                    expirationDate = card.ExpirationDate
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("byclient")]
        [Authorize(Policy = "BothPolicy")]
        public async Task<IActionResult> GetMembershipByClientIDAsync()
        {
            var subClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (subClaim == null)
                return Unauthorized(new { error = "Token inválido o claim 'sub' ausente." });

            var clientId = int.Parse(subClaim.Value);

            var card = await _membershipService.GetMembershipCardByClientIdAsync(clientId);

            if (card == null)
                return NotFound(new { error = "El cliente no posee una membresía activa." });

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
        // DTO para la request, que tome solo el ID del club y el plan ya que lo otro del user lo obtiene del JWT
        public class JoinClubRequest
        { 
            public int ClubId { get; set; } 
            public PartnerPlan Plan { get; set; }
        }
    }

}
