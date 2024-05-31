using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CaiNiaoSDK;
using CaiNiaoSDK.Http;
using CaiNiaoSDK.Models;
using CaiNiaoSDK.Models.Response;
using Newtonsoft.Json;

namespace PddOpenSDK
{
    public class DefaultCaiNiaoClient : CaiNiaoClient
    {
        internal HttpRequest HttpRequest { get; set; }

        public DefaultCaiNiaoClient(string appKey, string appSecret)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            HttpRequest = new HttpRequest();
        }

        public override TResponse Request<TResponse>(CaiNiaoRequestModel<TResponse> request, string accessToken = null)
        {
            IDictionary<string, string> paramDict = new Dictionary<string, string>();
            var logisticsInterface = JsonConvert.SerializeObject(request,Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var sign = BuildSign(logisticsInterface, "utf-8", AppSecret);
            paramDict.Add("logistics_interface", logisticsInterface);//业务内容
            paramDict.Add("logistic_provider_id", accessToken);//// 发送方的code 
            paramDict.Add("data_digest", sign);//签名
            paramDict.Add("to_code", "");//logisticsInterface
            paramDict.Add("msg_type", request.GetRequestTypeName());
            return HttpRequest.DoRequest<TResponse>(paramDict.AsEnumerable());
        }

        public override AccessTokenResponse AccessToken(string code)
        {
            var toSignContent = $"{code},{AppKey},{AppSecret}";
            var signContent = GetMd5Hash(toSignContent);
            var tokenUrl = string.Format(CaiNiaoUrlHelper.TokenUrl, code, AppKey, signContent);
            return HttpRequest.DoRequestAccessToken(tokenUrl);
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">需要加密的参数</param>
        /// <returns>返回16位小写字符串</returns>
        private string GetMd5Hash(String input)
        {
            if (input == null)
            {
                return null;
            }
            MD5 md5Hash = MD5.Create();
            // 将输入字符串转换为字节数组并计算哈希数据 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // 创建一个 Stringbuilder 来收集字节并创建字符串 
            StringBuilder sBuilder = new StringBuilder();
            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }            // 返回十六进制字符串 
            return sBuilder.ToString();
        }




        private string BuildSign(string content, string charset, string appSecret)
        {
            string toSignContent = content + appSecret;

            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.GetEncoding(charset).GetBytes(toSignContent);
            byte[] hash = md5.ComputeHash(inputBytes);

            return System.Convert.ToBase64String(hash);
        }



    }
}
