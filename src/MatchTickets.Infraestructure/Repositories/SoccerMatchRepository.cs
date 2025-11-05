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
    public class SoccerMatchRepository : GenericRepository<SoccerMatch>, ISoccerMatchRepository
    {
        public SoccerMatchRepository(DbContextCR context) : base(context)
        {
        }

        public async Task<IEnumerable<SoccerMatch>> GetAllAsync()
        {
            return await _context.SoccerMatches
                .Include(m => m.Club)
                .Include(m => m.Tickets)
                .ToListAsync();
        }

        public async Task<SoccerMatch?> GetByIdAsync(int matchId)
        {
            return await _context.SoccerMatches
                .Include(m => m.Club)
                .Include(m => m.Tickets)
                .FirstOrDefaultAsync(m => m.SoccerMatchId == matchId);
        }

        public async Task<IEnumerable<SoccerMatch>> GetByClubIdAsync(int clubId)
        {
            return await _context.SoccerMatches
                .Include(m => m.Club)
                .Include(m => m.Tickets)
                .Where(m => m.ClubId == clubId)
                .ToListAsync();
        }

    }

}
