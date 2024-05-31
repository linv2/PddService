using Newtonsoft.Json;
using PddOpenSDK.Attribute;
using PddOpenSDK.Models.Response.Logistics;

namespace PddOpenSDK.Models.Request.Logistics
{
    [PddRequestMethod("pdd.logistics.online.send")]
    public partial class SendLogisticsOnlineRequestModel : PddRequestModel<SendLogisticsOnlineResponseModel>
    {
        /// <summary>
        /// 发货个性内容，支持imei（手机串号），deviceSn（设备序列号）内容。形如：imei=11,22,3333; 以 “imei=” 开头，以英文分号“;”结尾，中间为手机IMEI串号信息，多个串号以英文逗号 “,”拼接释义：该订单包含三个手机IMEI串号，分别为11、22和3333；其他内容的格式同理。
        /// </summary>
        [JsonProperty("feature")]
        public string Feature { get; set; }
        /// <summary>
        /// 快递公司编号
        /// </summary>
        [JsonProperty("logistics_id")]
        public long LogisticsId { get; set; }
        /// <summary>
        /// 订单号。形如：20150909-452750051
        /// </summary>
        [JsonProperty("order_sn")]
        public string OrderSn { get; set; }
        /// <summary>
        /// 退货地址的id，不填则取商家默认地址
        /// </summary>
        [JsonProperty("refund_address_id")]
        public string RefundAddressId { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; }

    }

}
