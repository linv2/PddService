using System.Collections.Generic;
using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Mall
{
    public partial class PostMallInfoStoreDeleteRequestModel : PddRequestModel
    {
        /// <summary>
        /// 门店ID列表
        /// </summary>
        [JsonProperty("store_id_list")]
        public List<long> StoreIdList { get; set; }

    }

}
