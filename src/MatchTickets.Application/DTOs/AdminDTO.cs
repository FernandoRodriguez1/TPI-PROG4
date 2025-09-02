using MatchTickets.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class AdminDTO
    {

        public string UserName { get; set; }
        public string Email { get; set; }
       
        public string Password { get; set; }
       

    }
}
