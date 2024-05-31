using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Models.PmcUserPermit
{
   public class GetPddPmcUserPermitResponse : PddResponseModel
    {
        [JsonProperty("modified")]
        public string Modified { get; set; }


        [JsonProperty("created")]
        public string created { get; set; }


        [JsonProperty("is_expire")]
        public int isExpire { get; set; }


        [JsonProperty("owner_id")]
        public string OwnerId { get; set; }


        [JsonProperty("topics")]
        public string[] Topics { get; set; }
    }
}
