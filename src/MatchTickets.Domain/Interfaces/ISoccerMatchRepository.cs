using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Interfaces
{
    public interface ISoccerMatchRepository : IGenericRepository<SoccerMatch>
    {
        Task<SoccerMatch?> GetByIdAsync(int matchId);
        Task<IEnumerable<SoccerMatch>> GetAllAsync();
        Task<IEnumerable<SoccerMatch>> GetByClubIdAsync(int clubId);
    }

}
