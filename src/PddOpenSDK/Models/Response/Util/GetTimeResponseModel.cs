using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Util
{
    public partial class GetTimeResponseModel : PddResponseModel
    {
        /// <summary>
        /// response
        /// </summary>
        [JsonProperty("time_get_response")]
        public TimeGetResponseResponseModel TimeGetResponse { get; set; }
        public partial class TimeGetResponseResponseModel : PddResponseModel
        {
            /// <summary>
            /// 拼多多系统时间
            /// </summary>
            [JsonProperty("time")]
            public string Time { get; set; }

        }

    }

}
