using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PddOpenSDK.Http;
using PddOpenSDK.Models;
using PddOpenSDK.Models.Request;

namespace PddOpenSDK
{
    public class DefaultPddClient : PddClient
    {
        public HttpRequest HttpRequest { get; set; }

        public DefaultPddClient(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            HttpRequest = new HttpRequest();
        }

        public override TResponse Request<TResponse>(PddRequestModel<TResponse> request, string accessToken = null)
        {
            var dictionary = Function.ToDictionary(request);
            dictionary.Add("client_id", ClientId);
            dictionary.Add("data_type", "JSON");
            if (!string.IsNullOrEmpty(accessToken))
            {
                dictionary.Add("access_token", accessToken);
            }
            dictionary["timestamp"] = DateTimeOffset.Now.ToUnixTimeSeconds();
            dictionary["type"] = request.GetRequestTypeName();
            var paramsDic = BuildSign(dictionary);
            var requestBody = JsonConvert.SerializeObject(paramsDic);
            return HttpRequest.DoRequest<TResponse>(requestBody);
        }

        public override AccessTokenResponseModel AccessToken(string code,string redirect_uri, string state = null)
        {
            var requestBody = JsonConvert.SerializeObject(new AccessTokenRequestModel()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Code = code,
                GrantType = "authorization_code",
                RedirectUri = redirect_uri,
                State = state
            });
            return HttpRequest.DoRequest<AccessTokenResponseModel>(requestBody,PddUrlHelper.TokenUrl);
        }
        public override AccessTokenResponseModel RefreshToken(string refresh_token)
        {
            var requestBody = JsonConvert.SerializeObject(new RefreshTokenRequest()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                GrantType = "refresh_token",
                RefreshToken= refresh_token
            });
            return HttpRequest.DoRequest<AccessTokenResponseModel>(requestBody, PddUrlHelper.TokenUrl);
        }


        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public Dictionary<string, object> BuildSign(Dictionary<string, object> dictionary)
        {
            var result = new Dictionary<string, object>();
            // 去除空值并排序
            dictionary = dictionary.Where(d => d.Value != null)
                .OrderBy(d => d.Key)
                .ToDictionary((d) => d.Key, (d) => d.Value);
            // 拼接
            string signString = "";
            // 反射处理非基本类型字段的json转换
            string[] types = { "String", "DateTime", "Int64", "Boolean", "Float", "Double", "Long", "Int32" };
            foreach (var item in dictionary.Keys.ToArray())
            {
                if (!types.Contains(dictionary[item]?.GetType().Name))
                {
                    dictionary[item] = JsonConvert.SerializeObject(dictionary[item]);
                }
                dictionary.TryGetValue(item, out var value);
                // 布尔值大写造成的签名错误
                if (value.ToString().ToLower().Equals("false")) value = "false";
                if (value.ToString().ToLower().Equals("true")) value = "true";
                signString += item + value.ToString();
                result.Add(item, value.ToString());
            }
            signString = ClientSecret + signString + ClientSecret;
            using (var md5 = MD5.Create())
            {
                signString = Function.GetMd5Hash(md5, signString).ToUpper();
            }
            result.Add("sign", signString);
            return result;
        }

      
    }
}
