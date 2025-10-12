using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Interfaces
{
    public interface IClubRepository : IGenericRepository<Club>
    {
        Task<Club?> GetByIdAsync(int clubId);
        Task<IEnumerable<Club>> GetAllAsync();
        Task<int> GetMembersCountAsync(int clubId);
        Task<IEnumerable<SoccerMatch>> GetMatchesAsync(int clubId);
    }


}
