using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Goods
{
    public partial class CommitDeleteGoodsResponseModel : PddResponseModel
    {
        /// <summary>
        /// 操作结果
        /// </summary>
        [JsonProperty("open_api_response")]
        public bool? OpenApiResponse { get; set; }

    }

}
