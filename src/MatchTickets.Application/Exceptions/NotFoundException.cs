using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public string ErrorCode { get; private set; }
        public NotFoundException()
        : base()
        {
        }

        public NotFoundException(string message, string errorCode = "")
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
