using MatchTickets.Domain.Enums;
using MatchTickets.Domain.ValueObjects;
namespace MatchTickets.Domain.Entities

{
    public class Admin : User
    {
        private string v;
        private Email email;

        public Admin() : base()
        {
          UserType = UserType.Admin;
        }

        public Admin(string v, Email email)
        {
            this.v = v;
            this.email = email;
        }
    }
}
