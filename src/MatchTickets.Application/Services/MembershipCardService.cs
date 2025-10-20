﻿using MatchTickets.Application.Interfaces;
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

        public async Task<MembershipCard> CreateMembershipAsync(int clientId, int clubId, PartnerPlan plan)
        {
            // verifica si el cliente ya tiene un carnet activo
            var existingCard = await _membershipRepository.GetByClientIdAsync(clientId);
            if (existingCard != null)
                throw new InvalidOperationException("El cliente ya tiene un carnet activo.");

            // autoincrementa el numero del carnet 
            var allCards = await _membershipRepository.GetAllAsync();
            var lastCard = allCards
                .Where(c => c.ClubId == clubId)
                .OrderByDescending(c => c.MembershipId)
                .FirstOrDefault();

            int nextNumber = 1;
            if (lastCard != null)
            {
                var parts = lastCard.MembershipCardNumber.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
                    nextNumber = lastNumber + 1;
            }

            string newCardNumber = $"N-0000-{nextNumber:D4}";

            // crear la nueva Membership
            var newCard = new MembershipCard(clientId, clubId, plan)
            {
                MembershipCardNumber = newCardNumber,
                DischargeDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1))
            };

            await _membershipRepository.AddAsync(newCard);
            await _membershipRepository.SaveChangesAsync();

            // trae datos del cliente y club para enviar el correo personalizado
            var client = await _userRepository.GetClientByIdAsync(clientId);
            var club = await _clubRepository.GetByIdAsync(clubId);

            if (client != null && club != null)
            {
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
                    throw new InvalidOperationException($"No se pudo enviar el correo al cliente {client.UserName} ({client.Email.Value}): {ex.Message}", ex);
                }
            }

            return newCard;
        }



        public async Task<MembershipCard?> GetMembershipCardByClientIdAsync(int clientId)
        {
            return await _membershipRepository.GetByClientIdAsync(clientId);
        }
    }


}
