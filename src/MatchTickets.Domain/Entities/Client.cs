using MatchTickets.Domain.Enums;
using MatchTickets.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Entities
{
    public class Client : User
    {
        public Client()
        {
            UserType = UserType.Client;
            
        }
        
        public int Age { get; set; }
        public int PhoneNumber { get; set; }
        public int Dni { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();


        public int MembershipCardID { get; set; }
        public MembershipCard? MembershipCard { get; set; }
    }

}
