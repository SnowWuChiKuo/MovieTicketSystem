using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace ServerSide.Models.Infra
{
    public class HashUtility
    {
        private static IConfiguration configuration;

        public static void SetConfiguration(IConfiguration config)
        {
            configuration = config;
        }
        public static string ToSHA256(string plainText, string salt)
        {
            using (var mySHA256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(salt + plainText);
                var hash = mySHA256.ComputeHash(passwordBytes);
                var sb = new StringBuilder();
                foreach (var b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static string GetSalt()
        {
            if (configuration == null)
            {
                throw new InvalidOperationException("Configuration has not been set");
            }
            return configuration["AppSettings:salt"];
        }

        internal static bool VerifySHA256(string password, string encryptedPassword)
        {
            var hashPwd = ToSHA256(password, GetSalt());
            return hashPwd == encryptedPassword;
        }
    }
}
