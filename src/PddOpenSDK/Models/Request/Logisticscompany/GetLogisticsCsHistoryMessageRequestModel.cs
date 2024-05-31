using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Logisticscompany
{
    public partial class GetLogisticsCsHistoryMessageRequestModel : PddRequestModel
    {
        /// <summary>
        /// pdd会话id
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionId { get; set; }

    }

}
