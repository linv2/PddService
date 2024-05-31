using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Sms
{
    public partial class QuerySmsShortStatisticRequestModel : PddRequestModel
    {
        /// <summary>
        /// 任务id
        /// </summary>
        [JsonProperty("setting_id")]
        public long SettingId { get; set; }

    }

}
