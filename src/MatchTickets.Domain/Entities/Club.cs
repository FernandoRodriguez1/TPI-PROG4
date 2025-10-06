using MatchTickets.Domain.Entities;
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
    public class Club
    {
        [Key]
        public int ClubId { get; set; }

        public string ClubName { get; set; }

        public string Ubication { get; set; }

        public DateOnly FoundationOfTheClub { get; set; }

        public int StadiumCapacity  { get; set; }

        public List<MembershipCard> MembershipCards { get; set; }

        [NotMapped]
        public int MembershipCount { get; set; }


        public List<SoccerMatch> SoccerMatches { get; set; }

    }
}
