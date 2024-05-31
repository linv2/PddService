using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Subscribe
{
    public class PushMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("mallID")]
        public long MallId { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
