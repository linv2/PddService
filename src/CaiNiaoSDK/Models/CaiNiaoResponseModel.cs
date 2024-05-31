using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models
{
    public class CaiNiaoResponseModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty("errorMsg")]
        public string ErrorMsg { get; set; }
    }
}
