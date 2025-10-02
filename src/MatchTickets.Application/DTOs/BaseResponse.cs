using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.DTOs
{
    public class BaseResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
}
