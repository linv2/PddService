using System.Collections.Generic;
using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Goods
{
    public partial class GetGoodsCommitStatusRequestModel : PddRequestModel
    {
        /// <summary>
        /// goods_commit_id列表
        /// </summary>
        [JsonProperty("goods_commit_id_list")]
        public List<long> GoodsCommitIdList { get; set; }

    }

}
