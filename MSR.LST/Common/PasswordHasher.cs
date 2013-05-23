using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace MSR.LST
{
    public class PasswordHasher
    {
        private static PasswordHasher instance = new PasswordHasher();

        private readonly HashAlgorithm hashAlgorithm = SHA256.Create();
        private readonly Encoding textEncoder = Encoding.UTF8;

        private static readonly String randomJunk = "A bunch of randomish-looking bytes that we append to the end of each password";

        // singleton
        private PasswordHasher() { }

        public static PasswordHasher getInstance()
        {
            return instance;
        }

        public byte[] HashPassword(String password)
        {
            if (password == null || password.Length == 0)
                return null;

            return hashAlgorithm.ComputeHash(textEncoder.GetBytes(password + randomJunk));
        }
    }
}
