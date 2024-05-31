using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Refund
{
    public partial class GetRefundInformationRequestModel : PddRequestModel
    {
        /// <summary>
        /// 售后单id
        /// </summary>
        [JsonProperty("after_sales_id")]
        public long AfterSalesId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [JsonProperty("order_sn")]
        public string OrderSn { get; set; }

    }

}
