using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Goods
{
    public partial class GetGoodsDetailRequestModel : PddRequestModel
    {
        /// <summary>
        /// 1213414
        /// </summary>
        [JsonProperty("goods_id")]
        public long GoodsId { get; set; }

    }

}
