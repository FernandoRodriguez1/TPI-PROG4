using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class TicketDTO
    {
        public int TicketId { get; set; }

        public int? ClientId { get; set; } 
        public string? ClientMembershipNumber { get; set; } 

        public int SoccerMatchId { get; set; }
        public string? SoccerMatchDescription { get; set; } 
    }

}
