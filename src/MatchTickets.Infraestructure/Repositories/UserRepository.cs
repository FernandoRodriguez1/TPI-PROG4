using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Interfaces;
using MatchTickets.Domain.ValueObjects;
using MatchTickets.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MatchTickets.Infraestructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContextCR context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Admin>> GetAdminsAsync()
        {
            // filtra solo los Admin usando el tipo concreto
            return await _context.Users
                                 .OfType<Admin>()
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            // filtra solo los Client usando el tipo concreto
            return await _context.Users
                                 .OfType<Client>()
                                 .Include(c => c.MembershipCard)
                                 .ToListAsync();
        }

        public Task<Client?> GetClientByEmailAsync(Email email)
        {
            return _context.Users
                           .OfType<Client>()
                           .SingleOrDefaultAsync(u => u.Email.Value == email.Value);
        }


        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _context.Users
                                 .OfType<Client>()       // filtra solo Clients
                                 .FirstOrDefaultAsync(c => c.UserId == id); // busca por id
        }

        public User GetUserByEmail(Email email)
        {
            return _context.Users.FirstOrDefault(c => c.Email.Value == email.Value);
        }

    }
}
