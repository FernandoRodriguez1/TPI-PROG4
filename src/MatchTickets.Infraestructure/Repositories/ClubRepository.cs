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
    public class ClubRepository : IClubRepository
    {
        private readonly DbContextCR _context;

        public ClubRepository(DbContextCR context)
        {
            _context = context;
        }

        public async Task AddAsync(Club club)
        {
            if (club == null)
                throw new ArgumentNullException(nameof(club));

            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int clubId)
        {
            var club = await _context.Clubs.FindAsync(clubId);
            if (club != null)
            {
                _context.Clubs.Remove(club);
                await _context.SaveChangesAsync();
            }
           
        }

        public async Task<IEnumerable<Club>> GetAllAsync()
        {
            return await _context.Clubs
                .Include(c => c.MembershipCards)
                .Include(c => c.SoccerMatches)
                .ToListAsync();
        }

        public async Task<Club?> GetByIdAsync(int clubId)
        {
            return await _context.Clubs
                .Include(c => c.MembershipCards)
                .Include(c => c.SoccerMatches)
                .FirstOrDefaultAsync(c => c.ClubId == clubId);
        }

        public async Task<IEnumerable<SoccerMatch>> GetMatchesAsync(int clubId)
        {
            var club = await _context.Clubs
                .Include(c => c.SoccerMatches)
                .FirstOrDefaultAsync(c => c.ClubId == clubId);

            return club?.SoccerMatches ?? Enumerable.Empty<SoccerMatch>();
        }

        public async Task<int> GetMembersCountAsync(int clubId)
        {
            var club = await _context.Clubs
                .Include(c => c.MembershipCards)
                .FirstOrDefaultAsync(c => c.ClubId == clubId);

            return club?.MembershipCards.Count ?? 0;
        }

        public async Task UpdateAsync(Club club)
        {
            if (club == null)
                throw new ArgumentNullException(nameof(club));

            var existingClub = await _context.Clubs.FindAsync(club.ClubId);
            if (existingClub != null) 
            {
                _context.Entry(existingClub).CurrentValues.SetValues(club);
                await _context.SaveChangesAsync();
            }
            
        }
    }

}
