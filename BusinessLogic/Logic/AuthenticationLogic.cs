using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public static class AuthenticationLogic
    {
        public static string HashPassword(string password, string login)
        {
            SHA256 sha256 = new SHA256Managed();
            var saltedPassword = login + password;
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            var hash = Convert.ToBase64String(bytes);
            return hash;
        }   
     
        public static async Task<string> AuthenticateUser(string login, string password)
        {
            var u = await new UserLogic().GetUser(login);
            if (u == null)
                return "Unauthorized";
            var hash = HashPassword(password, login);
            return hash != u.Password ? "Unauthorized" : u.Login;
        }
    }
}