using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TopFun.Engine.Helpers
{
    public class Managment
    {
        public static string Generate(TypeGenerator type)
        {
            if (type == TypeGenerator.Login)
            {
                using (var random = new RNGCryptoServiceProvider())
                {
                    var buffer = new byte[5];
                    random.GetBytes(buffer);
         
                    return Convert.ToBase64String(buffer) + "topFun";
                }
            }

            if (type == TypeGenerator.Password)
            {
                var random = new Random();
                var result = new StringBuilder(7);
                var write = FromTo('a', 'w')
                    .Concat(FromTo('A', 'W'))
                    .Concat(FromTo('0', '9'))
                    .ToArray().ToList();

                for (int i = 0; i < 7; i++)
                {
                    var index = random.Next(0, write.Count);
                    result.Append(write[index]);
                }

                return result.ToString();
            }

            return Generate(0);
        }
        public static IEnumerable<char> FromTo(char start, char end)
        {
            if (start >= end)
                throw new ArgumentException("Начальное значение больше конечного.");
            for (int i = start; i < end; i++)
                yield return (char)i;
        }
        public static string GenerateComment()
        {
            return new Random().Next(100, 999).ToString();
        }
        public enum TypeGenerator
        {
            Login, 
            Password
        }

        public static string GetHWID()
        {
            var mc = new ManagementClass("win32_processor");
            var moc = mc.GetInstances();
            var id = string.Empty;
            foreach (var mo in moc)
            {
                id = mo.Properties["processorID"].Value.ToString();
                break;
            }
            return id;
        }
        public static string GetUserName()
        {
            return Environment.UserName;
        }
    }
}
