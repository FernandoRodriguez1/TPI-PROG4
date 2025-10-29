using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Interfaces;
using MatchTickets.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Infraestructure.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly DbContextCR _context;

        public TicketRepository(DbContextCR context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByClientIdAsync(int clientId)
        {
            return await _context.Tickets
                .Include(t => t.Client)
                    .ThenInclude(c => c.MembershipCard)
                .Include(t => t.SoccerMatch)
                    .ThenInclude(sm => sm.Club)
                .Where(t => t.ClientId == clientId)
                .ToListAsync();
        }
        public async Task<int> GetAvailableTicketsCountAsync(int matchId)
        {
            var match = await _context.SoccerMatches
                .Include(m => m.Tickets)
                .FirstOrDefaultAsync(m => m.SoccerMatchId == matchId);

            if (match == null) return 0;

            // number of available tickets = max - tickets vendidos
            var ticketsSold = match.Tickets.Count(t => !t.IsAvailable);
            return match.MaxTickets - ticketsSold;
        }


        public async Task<bool> ClientHasTicketAsync(int clientId, int matchId)
        {
            return await _context.Tickets
                .AnyAsync(t => t.ClientId == clientId && t.SoccerMatchId == matchId);
        }

        public async Task<Ticket> CreateTicketAsync(int clientId, int matchId)
        {
            // Verificar que el cliente ya no tenga un ticket para este partido
            var hasTicket = await ClientHasTicketAsync(clientId, matchId);
            if (hasTicket)
                throw new InvalidOperationException("El cliente ya tiene un ticket para este partido.");

            // Obtener el partido incluyendo el club y las membership cards
            var match = await _context.SoccerMatches
                .Include(sm => sm.Club)
                    .ThenInclude(c => c.MembershipCards)
                .FirstOrDefaultAsync(sm => sm.SoccerMatchId == matchId);

            if (match == null)
                throw new KeyNotFoundException("Partido no encontrado.");

            // Validar que el cliente tenga membership activa en ese club
            var membership = match.Club.MembershipCards.FirstOrDefault(mc => mc.ClientId == clientId);
            if (membership == null)
                throw new InvalidOperationException("El cliente no tiene una membership activa para este club.");


            // Crear el ticket
            var newTicket = new Ticket
            {
                ClientId = clientId,
                SoccerMatchId = matchId,
                IsAvailable = false
            };

            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();

            return newTicket;
        }
    }
}
