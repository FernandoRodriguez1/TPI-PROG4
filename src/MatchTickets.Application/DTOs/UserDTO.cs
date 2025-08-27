using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class UserDTO
    {
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
    }
}
