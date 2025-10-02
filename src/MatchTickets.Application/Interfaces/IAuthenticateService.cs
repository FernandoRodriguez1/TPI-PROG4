using MatchTickets.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Interfaces
{
    public interface IAuthenticateService
    {
        BaseResponse ValidateUser(AuthenticationRequestBody authenticationRequestBody);
    }
}
