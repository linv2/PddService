using Newtonsoft.Json;
using PddOpenSDK.Attribute;
using PddOpenSDK.Models.Response.Order;

namespace PddOpenSDK.Models.Request.Order
{
    [PddRequestMethod("pdd.erp.order.sync")]
    public partial class SyncErpOrderRequestModel : PddRequestModel<SyncErpOrderResponseModel>
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [JsonProperty("order_sn")]
        public string OrderSn { get; set; }
        /// <summary>
        /// 订单状态：1-已打单
        /// </summary>
        [JsonProperty("order_state")]
        public int OrderState { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        [JsonProperty("waybill_no")]
        public string WaybillNo { get; set; }
        /// <summary>
        /// 物流公司编码
        /// </summary>
        [JsonProperty("logistics_id")]
        public long LogisticsId { get; set; }

    }

}
