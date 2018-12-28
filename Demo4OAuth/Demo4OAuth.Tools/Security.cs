using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Demo4OAuth.Tools
{
    public class Security
    {
        public string EncryptionMD5(string value)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] tempdata = Encoding.Default.GetBytes(value);//将要加密的字符串转换为字节数组
            byte[] encryptdata = md5.ComputeHash(tempdata);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
        }

        public string EncryptionRSA(string value)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = "rsa_key";//密匙容器的名称，保持加密解密一致才能解密成功
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] tempdata = Encoding.Default.GetBytes(value);//将要加密的字符串转换为字节数组
                byte[] encryptdata = rsa.Encrypt(tempdata, false);//将加密后的字节数据转换为新的加密字节数组
                return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串
            }
        }

        public string DecryptRSA(string value)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = "rsa_key";
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] encryptdata = Convert.FromBase64String(value);
                byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                return Encoding.Default.GetString(decryptdata);
            }
        }

        public string EncryptionDES(string value)
        {
            byte[] buffer;
            DESCryptoServiceProvider DesCSP = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();//先创建 一个内存流
            CryptoStream cryStream = new CryptoStream(ms, DesCSP.CreateEncryptor(), CryptoStreamMode.Write);//将内存流连接到加密转换流
            StreamWriter sw = new StreamWriter(cryStream);
            sw.WriteLine(value);//将要加密的字符串写入加密转换流
            sw.Close();
            cryStream.Close();
            buffer = ms.ToArray();//将加密后的流转换为字节数组
            return Convert.ToBase64String(buffer);//将加密后的字节数组转换为字符串
        }

        public string DecryptDES(string value)
        {
            byte[] buffer = null;
            DESCryptoServiceProvider DesCSP = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(buffer);//将加密后的字节数据加入内存流中
            CryptoStream cryStream = new CryptoStream(ms, DesCSP.CreateDecryptor(), CryptoStreamMode.Read);//内存流连接到解密流中
            StreamReader sr = new StreamReader(cryStream);
            sr.Close();
            cryStream.Close();
            ms.Close();
            return sr.ReadLine();//将解密流读取为字符串
        }
    }
}
