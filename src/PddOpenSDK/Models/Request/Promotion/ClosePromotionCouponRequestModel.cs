using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Promotion
{
    public partial class ClosePromotionCouponRequestModel : PddRequestModel
    {
        /// <summary>
        /// 券批次ID
        /// </summary>
        [JsonProperty("batch_id")]
        public long BatchId { get; set; }

    }

}
