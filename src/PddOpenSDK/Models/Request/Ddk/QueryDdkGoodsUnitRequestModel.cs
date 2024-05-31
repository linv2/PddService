using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Ddk
{
    public partial class QueryDdkGoodsUnitRequestModel : PddRequestModel
    {
        /// <summary>
        /// 商品id
        /// </summary>
        [JsonProperty("goods_id")]
        public long GoodsId { get; set; }
        /// <summary>
        /// 招商duoId
        /// </summary>
        [JsonProperty("zs_duo_id")]
        public long? ZsDuoId { get; set; }

    }

}
