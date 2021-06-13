using System;
using System.Security.Cryptography;

namespace HashApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Started");
            //Console.WriteLine("Enter password");
            string passwordText = Console.ReadLine();

            byte[] salt = BitConverter.GetBytes(DateTime.Now.Ticks + DateTime.Now.Ticks);
            var salt2 = new byte[16];
            salt.CopyTo(salt2, 0);
            var hash = GeneratePasswordHashUsingSalt(passwordText, salt2);

            //Console.WriteLine(hash);
            //Console.WriteLine("Finished");
            Console.ReadKey();
        }

        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            byte[] hash = new Rfc2898DeriveBytes(passwordText, salt, 10000).GetBytes(20);

            byte[] hashBytes = new byte[36]; 
            
            Array.Copy(salt, 0, hashBytes, 0, 16); 
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;

        }
    }
}
