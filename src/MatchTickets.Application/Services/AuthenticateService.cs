using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        

        public AuthenticateService (IUserRepository userRepository, IPasswordHasher passwordHasher) 
        {
             _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public BaseResponse ValidateUser(AuthenticationRequestBody authenticationRequestBody)
        {
            var user = _userRepository.GetUserByEmail(authenticationRequestBody.Email);
            if (user == null)
            {
                return new BaseResponse { Result = false, Message = "Email not founded." };
            }

            var password = _passwordHasher.Verify(user.Password, authenticationRequestBody.Password);
            if (!password)
            {
                return new BaseResponse { Result = false, Message = "wrong password" };
            }

            return new BaseResponse { Result = true, Message = "success" };
        }
    }
}
