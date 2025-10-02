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
    public class UserRepository : IUserRepository
    {
        private readonly DbContextCR _context;

        public UserRepository(DbContextCR context)
        {
            _context = context;
        }

        public async Task AddAdminAsync(Admin admin)
        {
            _context.Users.Add(admin);
            await _context.SaveChangesAsync();

        }

        public async Task AddClientAsync(Client client)
        {
            _context.Users.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = new Client { UserId = id };
            _context.Users.Remove(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAdminAsync(int id)
        {
            var admin = new Admin { UserId = id };
            _context.Users.Remove(admin);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Admin>> GetAdminsAsync()
        {
            // Filtra solo los Admin usando el tipo concreto
            return await _context.Users
                                 .OfType<Admin>()
                                 .ToListAsync();

            // Alternativa usando el discriminador:
            // return await _context.Users
            //                      .Where(u => u.UserType == UserType.Admin)
            //                      .ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            // Filtra solo los Client usando el tipo concreto
            return await _context.Users
                                 .OfType<Client>()
                                 .Include(c => c.MembershipCard)
                                 .ToListAsync();

            // Alternativa usando el discriminador:return await _context.Clients
            // return await _context.Users
            //                      .Where(u => u.UserType == UserType.Client)
            //                      .ToListAsync();
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
