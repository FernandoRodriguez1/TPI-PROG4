using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Enums;
using MatchTickets.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Services
{
    public class MembershipCardService : IMembershipCardService
    {
        private readonly IMembershipCardRepository _membershipRepository;

        public MembershipCardService(IMembershipCardRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task<MembershipCard> CreateMembershipAsync(int clientId, int clubId, PartnerPlan plan)
        {
          
            var existingCard = await _membershipRepository.GetByClientIdAsync(clientId);
            if (existingCard != null)
                throw new InvalidOperationException("El cliente ya tiene un carnet activo.");


            var allCards = await _membershipRepository.GetAllAsync();

            var lastCard = allCards
                .Where(c => c.ClubId == clubId)
                .OrderByDescending(c => c.MembershipId)
                .FirstOrDefault();

            // calculo del siguiente numero de forma autoincremental
            int nextNumber = 1;
            if (lastCard != null)
            {
                var parts = lastCard.MembershipCardNumber.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
                    nextNumber = lastNumber + 1;
            }

            string newCardNumber = $"N-0000-{nextNumber:D4}";

            //  crea la nueva Membership
            var newCard = new MembershipCard(clientId, clubId, plan)
            {
                MembershipCardNumber = newCardNumber,
                DischargeDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1))
            };

            
            await _membershipRepository.AddAsync(newCard);
            await _membershipRepository.SaveChangesAsync();

            return newCard;
        }

        
        public async Task<MembershipCard?> GetMembershipCardByClientIdAsync(int clientId)
        {
            return await _membershipRepository.GetByClientIdAsync(clientId);
        }
    }


}
