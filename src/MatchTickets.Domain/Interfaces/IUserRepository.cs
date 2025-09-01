using MatchTickets.Domain.Entities;
using MatchTickets.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAdminsAsync();
        Task<IEnumerable<User>> GetClientsAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(Email email);
        Task AddClientAsync(Client client);
        Task AddAdminAsync(Admin admin);
        Task DeleteAdminAsync(int id);

        Task DeleteClientAsync (int id);


    }
}
