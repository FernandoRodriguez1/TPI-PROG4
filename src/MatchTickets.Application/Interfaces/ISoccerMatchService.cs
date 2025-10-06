using MatchTickets.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Interfaces
{
    public interface ISoccerMatchService
    {
        Task<IEnumerable<SoccerMatchDTO>> GetAllMatchesAsync();
        Task<SoccerMatchDTO?> GetMatchByIdAsync(int matchId);
        Task<IEnumerable<SoccerMatchDTO>> GetMatchesByClubAsync(int clubId);
        Task AddMatchAsync(SoccerMatchDTO matchDto);
        Task UpdateMatchAsync(SoccerMatchDTO matchDto);
        Task DeleteMatchAsync(int matchId);
    }

}
