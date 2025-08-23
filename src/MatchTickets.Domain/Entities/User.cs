using MatchTickets.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public int Dni { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public int Age { get; set; }

        public Email Correo { get; set; }  
    }

}
