using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Stock
{
    public partial class UpdateStockWareSkuResponseModel : PddResponseModel
    {
        /// <summary>
        /// 操作成功：true
        /// </summary>
        [JsonProperty("open_api_response")]
        public bool? OpenApiResponse { get; set; }

    }

}
