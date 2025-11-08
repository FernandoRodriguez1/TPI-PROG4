using AutoMapper;
using MatchTickets.Application.DTOs;
using MatchTickets.Application.Exceptions;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Enums;
using MatchTickets.Domain.Interfaces;
using MatchTickets.Domain.ValueObjects;


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

            await _userRepository.AddAsync(admin);
            await _userRepository.SaveChangesAsync();
        }


        public async Task AddClientAsync(ClientDTO clientDto)
        {
            if (clientDto == null)
                throw new AppValidationException("El cliente no puede ser nulo.", "CLIENT_NULL");

            var client = _mapper.Map<Client>(clientDto);
            client.Password = _passwordHasher.HashPassword(clientDto.Password);

            await _userRepository.AddAsync(client);
            await _userRepository.SaveChangesAsync();
        }


        public async Task DeleteClientAsync(int id)
        {
            var client = await _userRepository.GetByIdAsync(id);

            if (client == null)
                throw new NotFoundException($"Cliente con ID {id} no encontrado.", "CLIENT_NOT_FOUND");

            _userRepository.Delete(client);
            await _userRepository.SaveChangesAsync();
        }
        public async Task UpdateClientAsync(int id, UpdateClientDTO dto)
        {
            var client = await _userRepository.GetByIdAsync(id);

            if (client == null || client.UserType != UserType.Client)
                throw new NotFoundException($"Cliente con ID {id} no encontrado.", "CLIENT_NOT_FOUND");

            if (!string.IsNullOrWhiteSpace(dto.Nombre))
                client.UserName = dto.Nombre;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                client.Email = new Email(dto.Email);

            if (!string.IsNullOrWhiteSpace(dto.Password))
                client.Password = _passwordHasher.HashPassword(dto.Password);

            _userRepository.Update(client);
            await _userRepository.SaveChangesAsync();
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

            if (user == null)
                throw new NotFoundException($"Usuario con email {email.Value} no encontrado.", "USER_NOT_FOUND");

            return user;
        }

        public async Task<ClientDTO> GetClientByEmailAsync(Email email)
        {
            var user = await _userRepository.GetClientByEmailAsync(email);

            if (user == null)
                throw new NotFoundException($"Cliente con email {email.Value} no encontrado.", "CLIENT_NOT_FOUND");

            return ToDto(user);
        }

        public async Task<ClientDTO> GetClientByIdAsync(int id)
        {
            var user = await _userRepository.GetClientByIdAsync(id);

            if (user == null)
                throw new NotFoundException($"Cliente con ID {id} no encontrado.", "CLIENT_NOT_FOUND");

            return ToDto(user);
        }
    }

}
