using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Stock
{
    public partial class UpdateStockWareResponseModel : PddResponseModel
    {
        /// <summary>
        /// 货品id
        /// </summary>
        [JsonProperty("open_api_response")]
        public long? OpenApiResponse { get; set; }

    }

}
