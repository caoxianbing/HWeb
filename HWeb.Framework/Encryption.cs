using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HWeb.Framework
{
    /// <summary>
    /// 加密，解密类
    /// </summary>
    public static class Encryption
    {
        //默认AES密钥向量 
        private static byte[] _iV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0x34, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static string AesKey = "hwebjiaodaaeskey"; //AES密钥,128位
        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <param name="strKey">密钥</param>
        /// <returns>返回加密后的密文字节数组</returns>
        public static string AESEncrypt(string Data)
        {
            MemoryStream mStream = new MemoryStream();
            RijndaelManaged aes = new RijndaelManaged();

            byte[] plainBytes = Encoding.UTF8.GetBytes(Data);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(AesKey.PadRight(bKey.Length)), bKey, bKey.Length);

            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            aes.IV = _iV;
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();

                //return Encoding.UTF8.GetString(mStream.ToArray());
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return "";
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字节数组</param>
        /// <param name="strKey">密钥</param>
        /// <returns>返回解密后的字符串</returns>
        public static string AESDecrypt(string Data)
        {
            //url传惨会把+变成空格，这里在换回来
            Byte[] encryptedBytes = Convert.FromBase64String(Data.Replace(" ", "+"));
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(AesKey.PadRight(bKey.Length)), bKey, bKey.Length);

            MemoryStream mStream = new MemoryStream(encryptedBytes);
            //mStream.Write( encryptedBytes, 0, encryptedBytes.Length );    
            //mStream.Seek( 0, SeekOrigin.Begin );    
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            aes.IV = _iV;
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            catch
            {
                return "";
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="pwdSalt"></param>
        /// <returns></returns>
        public static string GetUserPassword(string pwd, string pwdSalt)
        {
            if (string.IsNullOrEmpty(pwdSalt))
                return pwd;

            var md5 = MD5.Create();

            pwd = ComputeHash(md5, pwd + pwdSalt);
            pwd = ComputeHash(md5, pwd + pwdSalt);

            return pwd;
        }

        /// <summary>
        /// MD5加密具体方法
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        static string ComputeHash(this MD5 md5, string pass)
        {
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(pass));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
