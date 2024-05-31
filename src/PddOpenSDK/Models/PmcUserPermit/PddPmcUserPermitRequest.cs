using Newtonsoft.Json;
using PddOpenSDK.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Models.PmcUserPermit
{
    [PddRequestMethod("pdd.pmc.user.permit")]
   public class PddPmcUserPermitRequest: PddRequestModel<PddPmcUserPermitResponse>
    {
        [JsonProperty("topics")]
        public string Topics { get; set; }
    }
}
