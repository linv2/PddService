using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.DTO
{
    /// <summary>
    /// 电子面单打印信息
    /// </summary>
    public class WaybillCloudPrintDTO
    {
        /// <summary>
        /// 请求id（与参数传入时相同）
        /// </summary>
        [JsonProperty("objectId")]
        public String ObjectId { get; set; }
        /// <summary>
        /// 面单号
        /// </summary>
        [JsonProperty("waybillCode")]
        public String WaybillCode { get; set; }
        /// <summary>
        /// 云打印内容
        /// </summary>
        [JsonProperty("printData")]
        public String PrintData { get; set; }

    }
}
