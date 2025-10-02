using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; private set; } 
        private Email() { }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                throw new ArgumentException("Email inválido", nameof(value));

            Value = value;
        }

        public override string ToString() => Value;
    }

}
