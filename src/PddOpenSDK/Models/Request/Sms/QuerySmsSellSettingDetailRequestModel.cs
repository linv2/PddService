using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Sms
{
    public partial class QuerySmsSellSettingDetailRequestModel : PddRequestModel
    {
        /// <summary>
        /// 任务id
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

    }

}
