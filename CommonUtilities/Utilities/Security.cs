using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementSystem.HelperMethods
{
    public class Security
    {
        public static string ComputeHash(string input)
        {
            var cipher = input;

            for (int i = 1; i <= 3; i++)
            {
                var bytes = Encoding.UTF8.GetBytes(cipher);
                using (var hash = System.Security.Cryptography.SHA512.Create())
                {
                    var hashedInputBytes = hash.ComputeHash(bytes);
                    var hashedInputStringBuilder = new StringBuilder(128);
                    foreach (var b in hashedInputBytes)
                    {
                        hashedInputStringBuilder.Append(b.ToString("X2"));
                    }

                    cipher = hashedInputStringBuilder.ToString();
                }
            }
            return cipher.ToLower();
        }
    }
}
