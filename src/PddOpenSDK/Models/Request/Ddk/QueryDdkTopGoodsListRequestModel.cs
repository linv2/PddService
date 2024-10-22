using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Ddk
{
    public partial class QueryDdkTopGoodsListRequestModel : PddRequestModel
    {
        /// <summary>
        /// 推广位id
        /// </summary>
        [JsonProperty("p_id")]
        public string PId { get; set; }
        /// <summary>
        /// 从多少位置开始请求；默认值 ： 0，offset需是limit的整数倍，仅支持整页翻页
        /// </summary>
        [JsonProperty("offset")]
        public int? Offset { get; set; }
        /// <summary>
        /// 1-实时热销榜；2-实时收益榜
        /// </summary>
        [JsonProperty("sort_type")]
        public int? SortType { get; set; }
        /// <summary>
        /// 请求数量；默认值 ： 400
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

    }

}
