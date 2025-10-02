using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Interfaces
{
    public interface IMembershipCardRepository
    {
        void Add(MembershipCard membershipCard);
        MembershipCard? GetByClientId(int clientId);

        IEnumerable<MembershipCard> GetAll();
    }
}
