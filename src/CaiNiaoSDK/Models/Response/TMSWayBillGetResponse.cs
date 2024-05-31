using CaiNiaoSDK.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.Response
{
    /// <summary>
    /// 电子面单云打印取号接口
    /// </summary>
    public class TMSWaybillGetResponse: CaiNiaoResponseModel
    {
        /// <summary>
        /// 电子面单打印信息列表
        /// </summary>
        [JsonProperty("waybillCloudPrintResponseList")]
       public List<WaybillCloudPrintDTO> WaybillCloudPrintResponseList { get; set; }
    }
}
