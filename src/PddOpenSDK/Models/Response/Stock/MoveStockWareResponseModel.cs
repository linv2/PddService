using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Stock
{
    public partial class MoveStockWareResponseModel : PddResponseModel
    {
        /// <summary>
        /// 调整单ID
        /// </summary>
        [JsonProperty("open_api_response")]
        public long? OpenApiResponse { get; set; }

    }

}
