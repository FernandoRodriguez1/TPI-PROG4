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
    public class MembershipCardRepository : GenericRepository<MembershipCard>, IMembershipCardRepository
    {
        private readonly DbContextCR _context;

        public MembershipCardRepository(DbContextCR context) : base(context)
        {
            _context = context;
        }

        public Task<MembershipCard?> GetByClientIdAsync(int clientId)
        {
            return _context.MembershipCards
                .FirstOrDefaultAsync(mc => mc.ClientId == clientId);
        }
    }
}
