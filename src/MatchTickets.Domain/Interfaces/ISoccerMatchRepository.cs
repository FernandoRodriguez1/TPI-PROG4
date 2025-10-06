using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Interfaces
{
    public interface ISoccerMatchRepository
    {
        Task AddAsync(SoccerMatch soccerMatch);
        Task<SoccerMatch?> GetByIdAsync(int matchId);
        Task<IEnumerable<SoccerMatch>> GetAllAsync();
        Task<IEnumerable<SoccerMatch>> GetByClubIdAsync(int clubId);
        Task UpdateAsync(SoccerMatch soccerMatch);
        Task DeleteAsync(int matchId);
    }

}
