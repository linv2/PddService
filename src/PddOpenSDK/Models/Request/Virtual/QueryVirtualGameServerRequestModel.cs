using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Virtual
{
    public partial class QueryVirtualGameServerRequestModel : PddRequestModel
    {
        /// <summary>
        /// 游戏CODE
        /// </summary>
        [JsonProperty("goods_config_code")]
        public string GoodsConfigCode { get; set; }

    }

}
