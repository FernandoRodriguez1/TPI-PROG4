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
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        private ClientDTO ToDto(Client client)
        {
            return new ClientDTO
            {
                UserName = client.UserName,
                Email = client.Email.Value,
                Age = client.Age,
                Password = null, // nunca mostrar
                MembershipCardNumber = client is Client c ? c.MembershipCard?.MembershipCardNumber : null,
                PhoneNumber = client.PhoneNumber
            };
        }

        private UserDTO UserToDto(User user)
        {
            return new UserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.UserId
            };
        }

        public async Task AddAdminAsync(AdminDTO adminDto)
        {
            var admin = _mapper.Map<Admin>(adminDto);
            admin.Password = _passwordHasher.HashPassword(adminDto.Password);
            await _userRepository.AddAdminAsync(admin);
        }

        public async Task AddClientAsync(ClientDTO dto)
        {
            var client = new Client
            {
                UserName = dto.UserName,
                Email = new Email(dto.Email),
                Age = dto.Age,
                PhoneNumber = dto.PhoneNumber,
                Password = dto.Password
            };

            await _userRepository.AddClientAsync(client);

        }


        public Task DeleteClientAsync(int id)
        {
            return _userRepository.DeleteClientAsync(id);
        }

        public async Task<IEnumerable<AdminDTO>> GetAdminsAsync()
        {
            var admins = await _userRepository.GetAdminsAsync();
            return _mapper.Map<IEnumerable<AdminDTO>>(admins);
        }


        public async Task<IEnumerable<ClientDTO>> GetClientsAsync()
        {
            var clients = await _userRepository.GetClientsAsync();
            return clients.Select(c => ToDto(c));
        }

        public User GetUserByEmail(Email email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null) return null!;
            return user;
        }
        public async Task<ClientDTO> GetClientByEmailAsync(Email email)
        {
            var user = await _userRepository.GetClientByEmailAsync(email);
            if (user == null) return null!;
            return ToDto(user);
        }

        public async Task<ClientDTO> GetClientByIdAsync(int id)
        {
            var user = await _userRepository.GetClientByIdAsync(id);
            if (user == null) return null!;
            return ToDto(user);
        }

    }
}
