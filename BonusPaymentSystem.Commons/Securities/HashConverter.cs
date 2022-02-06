using BCrypt.Net;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Commons.Securities
{
    public static class HashConverter
    {
        public static string ConvertToHash(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
           /* byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: text,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));*/

        }

        public static bool  VerifyPassword(string enteredPassword, string storedPassword)
        {
            /*
            byte[] salt = new byte[128 / 8];
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));*/

            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);
        }
        /*
        public static string SimpleDecrypt(string encryptedMessage, byte[] cryptKey, byte[] authKey,
                   int nonSecretPayloadLength = 0)
        {
            if (string.IsNullOrWhiteSpace(encryptedMessage))
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

            var cipherText = Convert.FromBase64String(encryptedMessage);
            var plainText = SimpleDecrypt(cipherText, cryptKey, authKey, nonSecretPayloadLength);
            return plainText == null ? null : Encoding.UTF8.GetString(plainText);
        }
        */
    }


}
