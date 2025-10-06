using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class SoccerMatchDTO
    {
        public int SoccerMatchId { get; set; }
        public DateOnly DayOfTheMatch { get; set; }
        public TimeSpan TimeOfTheMatch { get; set; }
        public string MatchLocation { get; set; }
        public int NumberTicketsAvailable { get; set; }
        public int ClubId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ClubName { get; set; } 
    }


}
