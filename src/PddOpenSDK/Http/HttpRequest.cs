using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PddOpenSDK.Exception;
using PddOpenSDK.Models;

namespace PddOpenSDK.Http
{
    public class HttpRequest
    {
        /// <summary>
        /// 请求接口
        /// </summary>
        const string PddSrvUrl = "http://gw-api.pinduoduo.com/api/router";
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
        public TResponse DoRequest<TResponse>(string requestBody,string requestUrl=null)
        {
            if (string.IsNullOrEmpty(requestUrl))
            {
                requestUrl = PddSrvUrl;
            }
            var stringContent=new StringContent(requestBody,Encoding.UTF8,"application/json");
            int retryTimes = 1;
            var response = httpClient.PostAsync(requestUrl, stringContent).Result;
            while (!response.IsSuccessStatusCode && AutoRetry && retryTimes < MaxRetryNumber)
            {
                response = httpClient.PostAsync(requestUrl, stringContent).Result;
                retryTimes++;
            }
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var jObject = JObject.Parse(responseContent);
                if (jObject.TryGetValue("error_response", out var error_response))
                {
                   var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(error_response.ToString());
                   throw new RequestException(errorResponse);
                }
                else
                {
                    return JsonConvert.DeserializeObject<TResponse>(responseContent);
                }
            }
            throw new ServiceException();
        }
    }
}
