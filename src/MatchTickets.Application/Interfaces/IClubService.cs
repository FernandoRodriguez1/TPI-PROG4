using MatchTickets.Application.DTOs;
using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Interfaces
{
    public interface IClubService
    {
        Task<IEnumerable<ClubDTO>> GetAllAsync();
        Task<Club?> GetByIdAsync(int clubId);
        Task<IEnumerable<SoccerMatchDTO>> GetMatchesAsync(int clubId);
        Task<int> GetMembersCountAsync(int clubId);
        Task AddAsync(ClubDTO club);
        Task UpdateAsync(ClubDTO club);
        Task DeleteAsync(int clubId);
    }
}
