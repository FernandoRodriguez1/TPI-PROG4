using MatchTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Entities
{
    public class MembershipCard 
    {
        [Key]
        public int MembershipId { get; set; }

        [Required]
        public PartnerPlan Plan { get; set; }

        public DateOnly DischargeDate { get; set; }

        public DateOnly ExpirationDate { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }


        public int ClubId { get; set; }
        public Club Club {  get; set; }

    }
}
