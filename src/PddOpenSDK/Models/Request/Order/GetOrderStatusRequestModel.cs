using Newtonsoft.Json;
using PddOpenSDK.Attribute;
using PddOpenSDK.Models.Response.Order;

namespace PddOpenSDK.Models.Request.Order
{
    [PddRequestMethod("pdd.order.status.get")]
    public partial class GetOrderStatusRequestModel : PddRequestModel<GetOrderStatusResponseModel>
    {
        /// <summary>
        /// 20150909-452750051,20150909-452750134 用逗号分开
        /// </summary>
        [JsonProperty("order_sns")]
        public string OrderSns { get; set; }

    }

}
