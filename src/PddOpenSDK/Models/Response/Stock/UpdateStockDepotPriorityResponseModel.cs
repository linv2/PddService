using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Stock
{
    public partial class UpdateStockDepotPriorityResponseModel : PddResponseModel
    {
        /// <summary>
        /// 请求结果
        /// </summary>
        [JsonProperty("open_api_response")]
        public string OpenApiResponse { get; set; }

    }

}
