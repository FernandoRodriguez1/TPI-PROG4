using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MatchTickets.Domain.Entities
{
    public class SoccerMatch
    {
        [Key]
        public int SoccerMatchId { get; set; }

        public DateOnly DayOfTheMatch { get; set; }

        public TimeSpan TimeOfTheMatch { get; set; }

        public string MatchLocation { get; set; }

        public int NumberTicketsAvailable { get; set; }

        public List<Ticket> Tickets { get; set; }

        public int ClubId { get; set; }
        public Club Club { get; set; }

        [NotMapped]
        public string? ClubName { get; set; }


    }
}
