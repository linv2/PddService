using Newtonsoft.Json;
using PddOpenSDK.Attribute;
using PddOpenSDK.Models.Response.Order;

namespace PddOpenSDK.Models.Request.Order
{
    [PddRequestMethod("pdd.order.information.get")]
    public partial class GetOrderInformationRequestModel : PddRequestModel<GetOrderInformationResponseModel>
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [JsonProperty("order_sn")]
        public string OrderSn { get; set; }

    }

}
