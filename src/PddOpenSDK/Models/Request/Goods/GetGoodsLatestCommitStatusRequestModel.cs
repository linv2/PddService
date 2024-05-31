using System.Collections.Generic;
using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Goods
{
    public partial class GetGoodsLatestCommitStatusRequestModel : PddRequestModel
    {
        /// <summary>
        /// 商品id(不超过100个)
        /// </summary>
        [JsonProperty("goods_id_list")]
        public List<long> GoodsIdList { get; set; }

    }

}
