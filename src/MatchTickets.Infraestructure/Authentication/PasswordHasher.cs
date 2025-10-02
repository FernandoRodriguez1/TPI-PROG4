using MatchTickets.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Infraestructure.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 128 / 8;
        private const int KeySize = 256 / 8;
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char Delimeter = ';';
        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(count: SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, outputLength: KeySize);

            return $"{Convert.ToBase64String(salt)}{Delimeter}{Convert.ToBase64String(hash)}";
        }

        public bool Verify(string passwordHash, string inputPassword)
        {
            var parts = passwordHash.Split(Delimeter);
            if (parts.Length != 2)
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, KeySize);

            return hashToCompare.SequenceEqual(hash);
        }
    }
}
