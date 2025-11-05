using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class MembershipCardResponse
    {
        public string Message { get; set; }
        public int MembershipId { get; set; }
        public string CardNumber { get; set; }
        public int ClubId { get; set; }
        public string Plan { get; set; }
        public DateOnly DischargeDate { get; set; }
        public DateOnly ExpirationDate { get; set; }
    }

}
