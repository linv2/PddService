using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Xml;
using CaiNiaoSDK.Models;
using CaiNiaoSDK.Models.Response;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CaiNiaoSDK.Http
{
    internal class HttpRequest
    {
        public ILog _logger = LogManager.GetLogger(typeof(HttpRequest));
        /// <summary>
        /// 请求接口
        /// </summary>
        const string PddSrvUrl = "http://link.cainiao.com/gateway/link.do";
        private HttpClient httpClient;
        public bool AutoRetry { get; set; } = true;
        public int MaxRetryNumber { get; set; } = 3;
        public double TimeOut
        {
            set => httpClient.Timeout = TimeSpan.FromSeconds(value);
            get => httpClient.Timeout.TotalSeconds;
        }
        public HttpRequest()
        {
            
            httpClient = new HttpClient(new HttpClientHandler() { UseProxy = false })
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }
        public TResponse DoRequest<TResponse>(IEnumerable<KeyValuePair<string, string>> nameValueCollection, string requestUrl = null) where TResponse : CaiNiaoResponseModel,new()
        {
            if (string.IsNullOrEmpty(requestUrl))
            {
                requestUrl = PddSrvUrl;
            }
            _logger.Debug($"POST {requestUrl}");
            var stringContent = new FormUrlEncodedContent(nameValueCollection);
            int retryTimes = 1;
            foreach(var header in stringContent.Headers)
            {
                _logger.DebugFormat($"{header.Key}:{string.Join(",", header.Value)}");
            }
            _logger.Debug(stringContent.ReadAsStringAsync().Result);
            var response = httpClient.PostAsync(requestUrl, stringContent).Result;
            while (!response.IsSuccessStatusCode && AutoRetry && retryTimes < MaxRetryNumber)
            {
                _logger.Debug($"{retryTimes} times request url fail，status code={(int)response.StatusCode},try again");
                response = httpClient.PostAsync(requestUrl, stringContent).Result;
                retryTimes++;
            }
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                if (responseContent.StartsWith("<response><success>"))
                {
                    var tResult = new TResponse();
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml (responseContent);
                    tResult.Success = Convert.ToBoolean(xmlDoc.SelectSingleNode("response/success").InnerText);
                    tResult.ErrorCode = xmlDoc.SelectSingleNode("response/errorCode").InnerText;
                    tResult.ErrorMsg = xmlDoc.SelectSingleNode("response/errorMsg").InnerText;
                    return tResult;
                }
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
            _logger.Debug($"request url fail，status code：{(int)response.StatusCode}");
            throw new Exception();
        }

        public AccessTokenResponse DoRequestAccessToken(string requestUrl = null)
        {
            int retryTimes = 1;
            var response = httpClient.GetAsync(requestUrl).Result;
            while (!response.IsSuccessStatusCode && AutoRetry && retryTimes < MaxRetryNumber)
            {
                response = httpClient.GetAsync(requestUrl).Result;
                retryTimes++;
            }
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<AccessTokenResponse>(responseContent);

            }
            throw new Exception();
        }
    }
}
