using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Interfaces
{
    public interface IMembershipCardService
    {
        Task<MembershipCard> CreateMembershipAsync(int clientId, int clubId, PartnerPlan plan);

        Task<MembershipCard?> GetMembershipCardByClientIdAsync(int clientId);
    }

}
