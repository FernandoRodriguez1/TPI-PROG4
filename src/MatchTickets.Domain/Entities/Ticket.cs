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

        public int ClientId { get; set; }
        public Client Client { get; set; }


        public int SoccerMatchId { get; set; }
        public SoccerMatch SoccerMatch { get; set; }



    }
}
