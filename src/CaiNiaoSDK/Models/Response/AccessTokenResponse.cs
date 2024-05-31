using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.Response
{
    public class AccessTokenResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("accessTokens")]
        public List<_AccessToken> AccessTokens { get; set; }
    }
    public class _AccessToken
    {
        internal _AccessToken()
        {

        }

        [JsonProperty("accessCode")]
        public string AccessCode { get; set; }


        [JsonProperty("grantBy")]
        public string GrantBy { get; set; }

        [JsonProperty("expireDate")]
        public string ExpireDate { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("apiId")]
        public string ApiId { get; set; }
    }
}
