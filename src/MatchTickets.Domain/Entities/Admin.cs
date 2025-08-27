using MatchTickets.Domain.Enums;
namespace MatchTickets.Domain.Entities

{
    public class Admin : User
    {
        public Admin() : base()
        {
          UserType = UserType.Admin;
        }
    }
}
