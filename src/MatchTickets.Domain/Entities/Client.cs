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
        private string v;
        private Email email;

        public Client() : base()
        {
            UserType = UserType.Client;
        }

        public Client(string v, Email email)
        {
            this.v = v;
            this.email = email;
        }

        public int Age { get; set; }
        public int PhoneNumber { get; set; }
        public int Dni { get; set; }

        public List<Ticket> Tickets { get; set; }

        public int MembershipCardID { get; set; }
        public MembershipCard MembershipCard { get; set; }

    }
}
