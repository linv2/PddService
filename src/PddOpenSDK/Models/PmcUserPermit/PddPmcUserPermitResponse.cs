using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Models.PmcUserPermit
{
   public class PddPmcUserPermitResponse:PddResponseModel
    {
        [JsonProperty("is_success")]
        public bool isSuccess { get; set; }
    }
}
