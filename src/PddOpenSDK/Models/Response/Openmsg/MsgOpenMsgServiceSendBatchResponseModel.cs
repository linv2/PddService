using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Openmsg
{
    public partial class MsgOpenMsgServiceSendBatchResponseModel : PddResponseModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [JsonProperty("code")]
        public int? Code { get; set; }
        /// <summary>
        /// 状态原因
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        /// <summary>
        /// 回执id
        /// </summary>
        [JsonProperty("biz_id")]
        public string BizId { get; set; }

    }

}
