using MatchTickets.Application.DTOs;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<AdminDTO>> GetAdminsAsync();
        Task<IEnumerable<ClientDTO>> GetClientsAsync();
        Task<ClientDTO> GetClientByIdAsync(int id);
        Task<ClientDTO> GetClientByEmailAsync(Email email);

        public User GetUserByEmail (Email email);
        Task AddClientAsync(ClientDTO clientDto);
        Task AddAdminAsync(AdminDTO adminDto); 
        Task DeleteClientAsync(int id);
 
        //public void UpdateUser(int id, UserDTO user);
        //public void UpdatePassword(int id, UserDTO user);
    }
}
