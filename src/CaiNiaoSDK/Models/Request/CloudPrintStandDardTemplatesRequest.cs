using CaiNiaoSDK.Attribute;
using CaiNiaoSDK.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.Request
{
    [CaiNiaoRequestMethod("CLOUDPRINT_STANDARD_TEMPLATES")]
    public class CloudPrintStandDardTemplatesRequest : CaiNiaoRequestModel<CloudPrintStandDardTemplatesResponse>
    {
        /// <summary>
        /// 指定cpCode的标准模板
        /// </summary>
        [JsonProperty("cpCode")]
        public string CpCode { get; set; }
    }
}
