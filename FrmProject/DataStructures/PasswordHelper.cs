using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace FrmProject.DataStructures
{
    internal static class PasswordHelper
    {
        public static bool VerifyPassword(string storedHash, string password)
        {
            if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(password))
                return false;

            var hasher = new PasswordHasher<object>();

            try
            {
                var result = hasher.VerifyHashedPassword(null!, storedHash, password);
                return result != PasswordVerificationResult.Failed;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (CryptographicException)
            {
                return false;
            }
        }

        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null!, password);
        }
    }
}

