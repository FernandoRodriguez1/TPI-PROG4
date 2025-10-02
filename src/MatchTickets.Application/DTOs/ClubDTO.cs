using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class ClubDTO
    {
        public int ClubId { get; set; }

        public string ClubName { get; set; }

        public string Ubication { get; set; }

        public DateOnly FoundationOfTheClub { get; set; }

        public int StadiumCapacity { get; set; }
    }
}
