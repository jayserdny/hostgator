using System;
using System.Security.Cryptography;
using System.Text;

namespace MVC5_Seneca.App_Code
{
    public static class EncryptSHA256
    {
        public static string GenerateSALT(int length) //length of salt    
        {
           SHA256 mySHA256 = SHA256Managed.Create();
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var randNum = new Random();
            var chars = new char[length];
            var allowedCharCount = allowedChars.Length;
            for (var i = 0; i <= length - 1; i++)
            {
                chars[i] = allowedChars[randNum.Next(allowedChars.Length)];
            }
            return new string(chars);
        }        

        public static string EncodeSHA256(string pass, string salt) //encrypt password    
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pass);
            byte[] src = Encoding.UTF8.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA256");
            byte[] inArray = algorithm.ComputeHash(dst);
            //return Convert.ToBase64String(inArray);          
            return EncodeBytesSHA256(Convert.ToBase64String(inArray));
        }

        public static string EncodeBytesSHA256(string pass) //Encrypt using SHA-256
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            SHA256 sha256;
            //Instantiate SHA256CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
            sha256 = new SHA256CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = sha256.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string    
            return BitConverter.ToString(encodedBytes);
        }       
    }
}