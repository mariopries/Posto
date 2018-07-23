using System;
using System.Text;

namespace PostoWeb
{
    public class Base64Convert
    {
        public static string convertStringToBase64(string str)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string convertBase64ToString(string str)
        {
            var base64EncodedBytes = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
