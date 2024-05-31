using Newtonsoft.Json;

namespace PddOpenSDK.Models
{
    public class AccessTokenRequestModel:PddRequestModel<AccessTokenResponseModel>
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }


        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }


        [JsonProperty("grant_type")]
        public string GrantType { get; set; }


        [JsonProperty("code")]
        public string Code { get; set; }


        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }


        [JsonProperty("state")]
        public string State { get; set; }
    }
}
