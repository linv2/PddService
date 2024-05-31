using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Stock
{
    public partial class CreateStockWareResponseModel : PddResponseModel
    {
        /// <summary>
        /// 货品id
        /// </summary>
        [JsonProperty("open_api_response")]
        public int? OpenApiResponse { get; set; }

    }

}
