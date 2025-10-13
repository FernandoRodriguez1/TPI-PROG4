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
    public class ClubRepository : GenericRepository<Club>, IClubRepository 
    {
        public ClubRepository(DbContextCR context) : base(context)
        {
        }

        public async Task<Club?> GetByIdAsync(int clubId)
        {
            // carga de club con sus relaciones
            var club = await _context.Clubs
                .Include(c => c.MembershipCards)
                .Include(c => c.SoccerMatches)
                    .ThenInclude(m => m.Tickets)
                .FirstOrDefaultAsync(c => c.ClubId == clubId);

            if (club == null)
                return null;

            // calculo de las membership card del club 
            club.MembershipCount = club.MembershipCards?.Count ?? 0;

            // lleno ClubName y NumberTicketsAvailable en cada partido
            if (club.SoccerMatches != null)
            {
                foreach (var match in club.SoccerMatches)
                {
                    match.ClubName = club.ClubName;
                    match.NumberTicketsAvailable = match.Tickets?.Count(t => t.IsAvailable) ?? 0;

                    //  vacio la referencia al club para evitar ciclos
                    match.Club = null;
                }
            }

            // no necesito las cards completas por eso null
            club.MembershipCards = null;

            return club;
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
    }

}
