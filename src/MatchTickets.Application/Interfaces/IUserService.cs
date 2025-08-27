using MatchTickets.Application.DTOs;
using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAdminsAsync();
        Task<IEnumerable<User>> GetClientsAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByNameAsync(string name);
        Task AddClientAsync(Client client);
        Task AddAdminAsync(AdminDTO adminDto); 
        Task DeleteUserAsync(int id);
 
        //public void UpdateUser(int id, UserDTO user);
        //public void UpdatePassword(int id, UserDTO user);
    }
}
