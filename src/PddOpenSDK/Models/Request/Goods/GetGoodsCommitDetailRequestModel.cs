using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Goods
{
    public partial class GetGoodsCommitDetailRequestModel : PddRequestModel
    {
        /// <summary>
        /// 提交申请的序列id
        /// </summary>
        [JsonProperty("goods_commit_id")]
        public long GoodsCommitId { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [JsonProperty("goods_id")]
        public long GoodsId { get; set; }

    }

}
