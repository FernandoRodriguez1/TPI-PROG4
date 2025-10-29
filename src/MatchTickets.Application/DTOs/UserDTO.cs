using MatchTickets.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Email Email { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }

        public string PasswordHash { get; set; }
    }
}
