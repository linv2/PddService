using CaiNiaoSDK.Attribute;
using CaiNiaoSDK.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.Response
{
    public class CloudPrintStandDardTemplatesResponse: CaiNiaoResponseModel
    {

        [JsonProperty("data")]
        public List<StandardTemplateResult> Data { get; set; }
    }

    public class StandardTemplateResult
    {
        [JsonProperty("cpCode")]
        public string CpCode { get; set; }
        [JsonProperty("standardTemplateDOs")]
        public List<StandardTemplateDTO> StandardTemplateDTOs { get; set; }
    }
}
