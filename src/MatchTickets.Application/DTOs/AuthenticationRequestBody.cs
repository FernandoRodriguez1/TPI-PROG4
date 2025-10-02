using MatchTickets.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class AuthenticationRequestBody
    {
        [Required]
        public Email Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
