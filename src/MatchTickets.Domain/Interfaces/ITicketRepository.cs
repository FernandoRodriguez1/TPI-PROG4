using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Interfaces
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetTicketsByClientIdAsync(int clientId);

        Task<int> GetAvailableTicketsCountAsync(int matchId);

        Task<bool> ClientHasTicketAsync(int clientId, int matchId);

        Task<Ticket> CreateTicketAsync(int clientId, int matchId);
    }

}
