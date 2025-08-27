using AutoMapper;
using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IMapper _mapper;

        public async Task AddAdminAsync(AdminDTO adminDto)
        {
            var user = _mapper.Map<AdminDTO>(adminDto); 
            await _userRepository.AddAsync(adminDto);
        }

        public async Task AddClientAsync(Client client)
        {
            var user = _mapper.Map<User>(client); 
            await _userRepository.AddAsync(client);
        }

        public async Task DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAdminsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetClientsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
