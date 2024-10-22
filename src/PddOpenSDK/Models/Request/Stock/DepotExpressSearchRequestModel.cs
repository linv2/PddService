using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Stock
{
    public partial class DepotExpressSearchRequestModel : PddRequestModel
    {
        /// <summary>
        /// 仓库编码
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 10 分页数据size
        /// </summary>
        [JsonProperty("length")]
        public int Length { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 0 分页数据起始位置
        /// </summary>
        [JsonProperty("start")]
        public int Start { get; set; }

    }

}
