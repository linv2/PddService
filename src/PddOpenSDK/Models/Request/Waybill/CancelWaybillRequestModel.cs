using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Waybill
{
    public partial class CancelWaybillRequestModel : PddRequestModel
    {
        /// <summary>
        /// 电子面单号
        /// </summary>
        [JsonProperty("waybill_code")]
        public string WaybillCode { get; set; }
        /// <summary>
        /// 快递公司code
        /// </summary>
        [JsonProperty("wp_code")]
        public string WpCode { get; set; }

    }

}
