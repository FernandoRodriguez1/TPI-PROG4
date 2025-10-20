using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Interfaces
{   
    public interface IMailService
    {
        Task SendMembershipCreatedEmailAsync(string toEmail, string memberName, string clubName, string membershipNumber);
    }
}
