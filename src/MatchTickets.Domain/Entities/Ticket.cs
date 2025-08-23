using MatchTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Entities
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        public StadiumSector Sector { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }


        public int MatchId { get; set; }
        public Match Match { get; set; }



    }
}
