using MatchTickets.Domain.Enums;
using MatchTickets.Domain.ValueObjects;
namespace MatchTickets.Domain.Entities

{
    public class Admin : User
    {
        public Admin()
        {
            UserType = UserType.Admin;
        }
    }

}
