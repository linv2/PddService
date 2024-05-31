using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.DTO
{
    public class Item
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
