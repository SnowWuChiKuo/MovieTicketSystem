using System.Security.Cryptography;
using System.Text;

namespace ServerSide.Models.Infra
{
    public class HashUtility
    {

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
            return System.Configuration.ConfigurationManager.AppSettings["salt"];
        }

        internal static bool VerifySHA256(string password, string encryptedPassword)
        {
            var hashPwd = ToSHA256(password, GetSalt());
            return hashPwd == encryptedPassword;
        }
    }
}
