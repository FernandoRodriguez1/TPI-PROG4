using MatchTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
     public class JoinClubRequest
     { 
            public int ClubId { get; set; } 
            public PartnerPlan Plan { get; set; }
     }
}
