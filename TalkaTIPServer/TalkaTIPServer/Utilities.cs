using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TalkaTIPSerwer
{
    class Utilities
    {
        public static string hashBytePassHex(string password)
        {
            byte[] bytePasswd = Encoding.Default.GetBytes(password);
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytePasswd = sha256.ComputeHash(bytePasswd); //256-bits
                string hashBytePasswdHex = BitConverter.ToString(hashBytePasswd).Replace("-", string.Empty);//256 bit hash password
                return hashBytePasswdHex;
            }
        }
        public static string getBinaryMessage(string message)
        {
            string binMessage = string.Empty;
            foreach (char ch in message)
            {
                binMessage += Convert.ToString((int)ch, 2).PadLeft(8, '0');
            }
            return binMessage;
        }
    }
}
