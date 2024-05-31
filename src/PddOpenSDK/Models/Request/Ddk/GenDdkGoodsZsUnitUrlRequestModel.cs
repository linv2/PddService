using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Ddk
{
    public partial class GenDdkGoodsZsUnitUrlRequestModel : PddRequestModel
    {
        /// <summary>
        /// 需转链的链接
        /// </summary>
        [JsonProperty("source_url")]
        public string SourceUrl { get; set; }
        /// <summary>
        /// 渠道id
        /// </summary>
        [JsonProperty("pid")]
        public string Pid { get; set; }

    }

}
