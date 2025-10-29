using MatchTickets.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDTO>> GetTicketsByClientAsync(int clientId);
        Task<TicketDTO> BuyTicketAsync(int clientId, int matchId);

        
    }

}
