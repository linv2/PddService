using CaiNiaoSDK.Attribute;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.Request
{
    [CaiNiaoRequestMethod("TMS_WAYBILL_SUBSCRIPTION_QUERY")]
    public class TMSWaybillSubscriptionQueryRequest:CaiNiaoRequestModel<TMSWaybillSubscriptionQueryResponse>
    {
        /// <summary>
        /// 指定cpCode的标准模板
        /// </summary>
        [JsonProperty("cpCode")]
        public string CpCode { get; set; }
    }
}
