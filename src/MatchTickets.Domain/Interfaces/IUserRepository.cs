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
        Task<IEnumerable<Admin>> GetAdminsAsync();
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client?> GetClientByEmailAsync(Email email);
        Task AddClientAsync(Client client);
        Task AddAdminAsync(Admin admin);
        Task DeleteAdminAsync(int id);

        public User GetUserByEmail(Email email);
        Task DeleteClientAsync (int id);


    }
}
