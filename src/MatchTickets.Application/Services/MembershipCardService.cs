using MatchTickets.Application.DTOs;
using MatchTickets.Application.Exceptions;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Enums;
using MatchTickets.Domain.Interfaces;


namespace MatchTickets.Application.Services
{
    public class MembershipCardService : IMembershipCardService
    {
        private readonly IMembershipCardRepository _membershipRepository;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;

        public MembershipCardService(IMembershipCardRepository membershipRepository, IMailService mailService, IUserRepository userRepository, IClubRepository clubRepository)
        {
            _membershipRepository = membershipRepository;
            _mailService = mailService;
            _userRepository = userRepository;
            _clubRepository = clubRepository;
        }

        public async Task<MembershipCardResponse> CreateMembershipAsync(int clientId, int clubId, PartnerPlan plan)
        {
            
            var existingCard = await _membershipRepository.GetByClientIdAsync(clientId);
            if (existingCard != null)
                throw new AppValidationException("El cliente ya tiene un carnet activo.", "CLIENT_ALREADY_HAS_MEMBERSHIP");

            
            var lastCard = await _membershipRepository.GetLastMembershipOfClubAsync(clubId);

            int nextNumber = 1;
            if (lastCard != null)
            {
                var parts = lastCard.MembershipCardNumber.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
                    nextNumber = lastNumber + 1;
            }

            string newCardNumber = $"N-0000-{nextNumber:D4}";

           
            var newCard = new MembershipCard(clientId, clubId, plan)
            {
                MembershipCardNumber = newCardNumber,
                DischargeDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1))
            };

            await _membershipRepository.AddAsync(newCard);
            await _membershipRepository.SaveChangesAsync();

            
            var client = await _userRepository.GetClientByIdAsync(clientId)
                ?? throw new NotFoundException($"Cliente con ID {clientId} no encontrado.", "CLIENT_NOT_FOUND");

            var club = await _clubRepository.GetByIdAsync(clubId)
                ?? throw new NotFoundException($"Club con ID {clubId} no encontrado.", "CLUB_NOT_FOUND");

            
            try
            {
                await _mailService.SendMembershipCreatedEmailAsync(
                    toEmail: client.Email.Value,
                    memberName: client.UserName,
                    clubName: club.ClubName,
                    membershipNumber: newCard.MembershipCardNumber
                );
            }
            catch (Exception ex)
            {
                throw new AppValidationException(
                    $"No se pudo enviar el correo a {client.UserName} ({client.Email.Value}): {ex.Message}",
                    "EMAIL_SEND_FAILED"
                );
            }

            
            string message = clubId switch
            {
                1 => "¡Bienvenido Leproso! 🟥⬛",
                2 => "¡Bienvenido Canalla! 🟡🔵",
                3 => "¡Bienvenido Matador! 🔵⚪",
                _ => "¡Socio creado con éxito!"
            };

            
            return new MembershipCardResponse
            {
                Message = message,
                MembershipId = newCard.MembershipId,
                CardNumber = newCard.MembershipCardNumber,
                ClubId = newCard.ClubId,
                Plan = newCard.Plan.ToString(),
                DischargeDate = newCard.DischargeDate,
                ExpirationDate = newCard.ExpirationDate
            };
        }


        public async Task<MembershipCard> GetMembershipCardByClientIdAsync(int clientId)
        {
            var card = await _membershipRepository.GetByClientIdAsync(clientId);

            if (card is null)
                throw new NotFoundException("El cliente no posee una membresía activa.", "CLIENT_HAS_NO_MEMBERSHIP");

            return card;
        }

    }

}
