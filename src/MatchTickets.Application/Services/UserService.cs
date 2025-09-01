using AutoMapper;
using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Interfaces;
using MatchTickets.Domain.ValueObjects;
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
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task AddAdminAsync(AdminDTO adminDto)
        {
            var admin = _mapper.Map<Admin>(adminDto);
            await _userRepository.AddAdminAsync(admin);
        }


        public async Task AddClientAsync(ClientDTO clientDto)
        {
            var user = _mapper.Map<Client>(clientDto); 
            await _userRepository.AddClientAsync(user);
            
        }

        public Task DeleteClientAsync(int id)
        {
            return _userRepository.DeleteClientAsync(id);
        }

        public Task<IEnumerable<User>> GetAdminsAsync()
        {
            return _userRepository.GetAdminsAsync();
        }


        public Task<IEnumerable<User>> GetClientsAsync()
        {
            return _userRepository.GetClientsAsync();
        }


        public Task<User?> GetUserByEmailAsync(Email email)
        {
            return _userRepository.GetUserByEmailAsync(email);
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            return _userRepository.GetUserByIdAsync(id);
        }
    }
}
