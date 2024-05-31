using CaiNiaoSDK.Attribute;
using CaiNiaoSDK.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.Request
{
    public class TMSWaybillSubscriptionQueryResponse:CaiNiaoResponseModel
    {
        [JsonProperty("waybillApplySubscriptionCols")]
        public List<WaybillApplySubscriptionInfoDTO> WaybillApplySubscriptionCols { get; set; }
    }
}
