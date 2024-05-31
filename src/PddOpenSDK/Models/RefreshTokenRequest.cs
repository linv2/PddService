using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Models
{
   public class RefreshTokenRequest:PddRequestModel<AccessTokenResponseModel>
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }


        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }


        [JsonProperty("grant_type")]
        public string GrantType { get; set; }



        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
