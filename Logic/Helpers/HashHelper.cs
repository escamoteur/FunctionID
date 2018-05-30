using System;
using System.Security.Cryptography;
using System.Text;

namespace FunctionId.Logic.Helpers
{
    // source https://blogs.msdn.microsoft.com/alextch/2012/05/12/sample-c-code-to-create-sha1-salted-ssha-password-hashes-for-openldap/

    public class HashHelper
    {
        public static (string hashedAndSalted, String salt) GenerateSaltedSHA1(string plainTextString)
        {
            var sha1 = SHA1.Create();

            var saltBytes = GenerateSalt(10);

            var plainTextBytes = Encoding.ASCII.GetBytes(plainTextString);

            var plainTextWithSaltBytes = AppendByteArray(plainTextBytes, saltBytes);

            var saltedSHA1Bytes = sha1.ComputeHash(plainTextWithSaltBytes);

            return (Convert.ToBase64String(saltedSHA1Bytes), Convert.ToBase64String(saltBytes));
        }


        public static bool VerifyAgainstSaltedHash(string hashedAndSaltedString, string salt, string plainTextString)
        {
            var sha1 = SHA1.Create();

            var plainTextBytes = Encoding.ASCII.GetBytes(plainTextString);

            var plainTextWithSaltBytes = AppendByteArray(plainTextBytes, Convert.FromBase64String(salt));

            var saltedSHA1Bytes = sha1.ComputeHash(plainTextWithSaltBytes);


            var base64String = Convert.ToBase64String(saltedSHA1Bytes);
            return hashedAndSaltedString.Equals(base64String);
        }



        private static byte[] GenerateSalt(int saltSize)
        {
            var randomKeyGenerator = new RandomKeyGenerator();
            return randomKeyGenerator.GetBytes(saltSize);
        }

        private static byte[] AppendByteArray(byte[] byteArray1, byte[] byteArray2)
        {
            var byteArrayResult =
                new byte[byteArray1.Length + byteArray2.Length];

            for (var i = 0; i < byteArray1.Length; i++)
                byteArrayResult[i] = byteArray1[i];
            for (var i = 0; i < byteArray2.Length; i++)
                byteArrayResult[byteArray1.Length + i] = byteArray2[i];

            return byteArrayResult;
        }


    }
}
