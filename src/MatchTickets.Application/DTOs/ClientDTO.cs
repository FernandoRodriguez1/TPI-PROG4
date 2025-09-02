using MatchTickets.Domain.Entities;
using MatchTickets.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class ClientDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int PhoneNumber { get; set; }

        public string Password { get; set; }
        public string? MembershipCardNumber { get; set; }

    }
}
