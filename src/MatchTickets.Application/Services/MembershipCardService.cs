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
        private readonly IMembershipCardRepository _Membershiprepository;

        public MembershipCardService(IMembershipCardRepository repository)
        {
            _Membershiprepository = repository;
        }

        public MembershipCard CreateMembership(int clientId, int clubId, PartnerPlan plan)
        {
            // Verificar si el cliente ya tiene un carnet activo
            var existingCard = _Membershiprepository.GetByClientId(clientId);
            if (existingCard != null)
                throw new InvalidOperationException("El cliente ya tiene un carnet activo.");

            // Obtener la última MembershipCard del club directamente desde GetAll()
            var lastCard = _Membershiprepository
                .GetAll() // IEnumerable<MembershipCard>
                .Where(c => c.ClubId == clubId)
                .OrderByDescending(c => c.MembershipId) // o por DischargeDate si preferís
                .FirstOrDefault();

            // Calcular el siguiente número autoincremental
            int nextNumber = 1;
            if (lastCard != null)
            {
                var parts = lastCard.MembershipCardNumber.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
                    nextNumber = lastNumber + 1;
            }

            string newCardNumber = $"N-0000-{nextNumber:D4}";

            // Crear la nueva MembershipCard
            var newCard = new MembershipCard(clientId, clubId, plan)
            {
                MembershipCardNumber = newCardNumber,
                DischargeDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1))
            };

            // Guardar en el repositorio
            _Membershiprepository.Add(newCard);

            return newCard;
        }


    }
}
