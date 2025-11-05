using MatchTickets.Application.Exceptions;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Interfaces;
using MatchTickets.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;


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
            var match = await _context.SoccerMatches
                .Include(sm => sm.Club)
                    .ThenInclude(c => c.MembershipCards)
                .FirstOrDefaultAsync(sm => sm.SoccerMatchId == matchId);

            var membership = match.Club.MembershipCards.FirstOrDefault(mc => mc.ClientId == clientId);
            if (membership == null)
                throw new AppValidationException("El cliente no tiene una membership activa para este club.");

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
