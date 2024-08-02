using System.Security.Cryptography;
using System.Text;

namespace Common.Lib.Helpers
{
    public static class Md5Helper
    {
        public static string Md5(byte[] buffer)
        {
            using MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(buffer);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var t in data)
            {
                stringBuilder.Append(t.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string Md5(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return Md5(Encoding.UTF8.GetBytes(input));
        }
    }
}