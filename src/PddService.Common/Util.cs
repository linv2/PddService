using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PddService.Common
{
    public static class Util
    {
        /// <summary>
        /// SHA1 加密
        /// </summary>
        public static string Sha1(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            var encoding = Encoding.UTF8;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            return HashAlgorithmBase(sha1, value, encoding);
        }

        public static string Md5(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            var encoding = Encoding.UTF8;
            MD5 md5 = MD5.Create();
            return HashAlgorithmBase(md5, value, encoding);
        }

        /// <summary>
        /// SHA256 加密
        /// </summary>
        public static string Sha256(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            var encoding = Encoding.UTF8;
            SHA256 sha256 = new SHA256Managed();
            return HashAlgorithmBase(sha256, value, encoding);
        }


        /// <summary>
        /// HashAlgorithm 加密统一方法
        /// </summary>
        private static string HashAlgorithmBase(HashAlgorithm hashAlgorithmObj, string source, Encoding encoding)
        {
            byte[] btStr = encoding.GetBytes(source);
            byte[] hashStr = hashAlgorithmObj.ComputeHash(btStr);
            return hashStr.Bytes2Str();
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        private static string Bytes2Str(this IEnumerable<byte> source, string formatStr = "{0:X2}")
        {
            StringBuilder pwd = new StringBuilder();
            foreach (byte btStr in source)
            {
                pwd.AppendFormat(formatStr, btStr);
            }

            return pwd.ToString();
        }


        public static string WxAesDecrypt(string key, string iv, string text)
        {
            try
            {
                //判断是否是16位 如果不够补0
                //text = tests(text);
                //16进制数据转换成byte
                var encryptedData = Convert.FromBase64String(text); // strToToHexByte(text);
                var rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Key = Convert.FromBase64String(key); // Encoding.UTF8.GetBytes(AesKey);
                rijndaelCipher.IV = Convert.FromBase64String(iv); // Encoding.UTF8.GetBytes(AesIV);
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                var transform = rijndaelCipher.CreateDecryptor();
                var plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                var result = Encoding.Default.GetString(plainText);
                //int index = result.LastIndexOf('>');
                //result = result.Remove(index + 1);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static JToken DecryptWxData(JToken jToken)
        {
                var iv = jToken["iv"].Value<string>();
                var encryptedData = jToken["encryptedData"].Value<string>();
                var sessionKey = jToken["sessionKey"].Value<string>();
                var data = Util.WxAesDecrypt(sessionKey, iv, encryptedData);
                var weToken = JToken.Parse(data);
                return weToken;
        }

        private static readonly char[] RandRonstant =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
        };

        public static string GenerateRandomNumber(int length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(RandRonstant[rd.Next(10)]);
            }

            return newRandom.ToString();
        }

        public static long GetTimestamp(this DateTime dateTime)
        {
            var timeSpan = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1);//ToUniversalTime()转换为标准时区的时间,去掉的话直接就用北京时间
         //   return (long)ts.TotalMilliseconds; //精确到毫秒
            return (long)timeSpan.TotalSeconds;//获取10位
        }
    }
}