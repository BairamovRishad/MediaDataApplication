using System.Security.Cryptography;
using System.Text;

namespace MediaDataApplication.WcfService.Logic {

    public class Security {
        public static string Encrypt(string plainText) {
            var x = new MD5CryptoServiceProvider();
            byte[] data = Encoding.ASCII.GetBytes(plainText);
            data = x.ComputeHash(data);
            return Encoding.ASCII.GetString(data);
        }

        public static bool Match(string cipher, string plainText) {
            var encryptedText = Encrypt(plainText);
            return encryptedText == cipher;
        }
    }

}