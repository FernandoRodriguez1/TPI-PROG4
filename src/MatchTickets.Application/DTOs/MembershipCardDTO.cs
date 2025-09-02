using MatchTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class MembershipCardDTO
    {
        public int MembershipId { get; set; }
        public string MembershipCardNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public PartnerPlan Plan { get; set; }

        public DateOnly DischargeDate { get; set; }

        public DateOnly ExpirationDate { get; set; }
    }
}
