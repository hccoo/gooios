using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Gooios.Infrastructure.Secrets
{
    public class SecretProvider
    {
        public static string EncryptToMD5(string str)
        {
            byte[] buffer = Encoding.Default.GetBytes(str);
            var md5 = MD5.Create();
            byte[] bufferNew = md5.ComputeHash(buffer);
            string strNew = null;
            for (int i = 0; i < bufferNew.Length; i++)
            {
                strNew += bufferNew[i].ToString("x2");
            }
            return strNew;
        }
    }
}
