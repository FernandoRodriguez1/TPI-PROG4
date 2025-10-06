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
    public class SoccerMatchRepository : ISoccerMatchRepository
    {
        private readonly DbContextCR _context;

        public SoccerMatchRepository(DbContextCR context)
        {
            _context = context;
        }

        public async Task AddAsync(SoccerMatch soccerMatch)
        {
            await _context.SoccerMatches.AddAsync(soccerMatch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int matchId)
        {
            var match = await _context.SoccerMatches.FindAsync(matchId);
            if (match != null)
            {
                _context.SoccerMatches.Remove(match);
                await _context.SaveChangesAsync();
            }
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

        public async Task UpdateAsync(SoccerMatch soccerMatch)
        {
            _context.SoccerMatches.Update(soccerMatch);
            await _context.SaveChangesAsync();
        }
    }

}
