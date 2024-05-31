using Newtonsoft.Json;
using PddOpenSDK.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Models.PmcUserPermit
{
    [PddRequestMethod("pdd.pmc.user.get")]
   public class GetPddGetPmcUserPermitRequest : PddRequestModel<GetPddPmcUserPermitResponse>
    {
        [JsonProperty("owner_id")]
        public string OwnerId { get; set; }
    }
}
