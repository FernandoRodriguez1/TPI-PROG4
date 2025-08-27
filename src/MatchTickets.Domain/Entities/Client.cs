using MatchTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Entities
{
    public class Client : User
    {
        public Client() : base()
        {
            UserType = UserType.Client;
        }
        public int Age { get; set; }
        public int PhoneNumber { get; set; }
        public int Dni { get; set; }

    }
}
