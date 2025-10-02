using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Interfaces;
using MatchTickets.Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Infraestructure.Repositories
{
    public class MembershipCardRepository : IMembershipCardRepository
    {
        private readonly DbContextCR _context;

        public MembershipCardRepository(DbContextCR context)
        {
            _context = context;
        }

        public void Add(MembershipCard membershipCard)
        {
            _context.MembershipCards.Add(membershipCard);
            _context.SaveChanges();
        }

        public MembershipCard? GetByClientId(int clientId)
        {
            return _context.MembershipCards.FirstOrDefault(m => m.ClientId == clientId);
        }

        public IEnumerable<MembershipCard> GetAll()
        {
            return _context.MembershipCards.ToList();
        }
    }
}
