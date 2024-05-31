using System.Collections.Generic;
using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Ddk
{
    public partial class GenDdkWeappQrcodeUrlRequestModel : PddRequestModel
    {
        /// <summary>
        /// 推广位ID
        /// </summary>
        [JsonProperty("p_id")]
        public string PId { get; set; }
        /// <summary>
        /// 商品ID，仅支持单个查询
        /// </summary>
        [JsonProperty("goods_id_list")]
        public List<long> GoodsIdList { get; set; }
        /// <summary>
        /// 自定义参数，为链接打上自定义标签。自定义参数最长限制64个字节。
        /// </summary>
        [JsonProperty("custom_parameters")]
        public string CustomParameters { get; set; }
        /// <summary>
        /// 招商多多客ID
        /// </summary>
        [JsonProperty("zs_duo_id")]
        public long? ZsDuoId { get; set; }
        /// <summary>
        /// 是否生成店铺收藏券推广链接
        /// </summary>
        [JsonProperty("generate_mall_collect_coupon")]
        public bool? GenerateMallCollectCoupon { get; set; }

    }

}
