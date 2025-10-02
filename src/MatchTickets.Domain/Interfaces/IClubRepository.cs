using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Interfaces
{
    public interface IClubRepository
    {
        Task AddAsync(Club club);
        Task<Club?> GetByIdAsync(int clubId);
        Task<IEnumerable<Club>> GetAllAsync();
        Task<int> GetMembersCountAsync(int clubId);
        Task<IEnumerable<SoccerMatch>> GetMatchesAsync(int clubId);
        Task UpdateAsync(Club club);
        Task DeleteAsync(int clubId);
    }


}
