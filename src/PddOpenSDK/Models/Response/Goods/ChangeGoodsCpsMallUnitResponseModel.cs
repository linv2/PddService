using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Goods
{
    public partial class ChangeGoodsCpsMallUnitResponseModel : PddResponseModel
    {
        /// <summary>
        /// response
        /// </summary>
        [JsonProperty("goods_cps_mall_unit_change_response")]
        public GoodsCpsMallUnitChangeResponseResponseModel GoodsCpsMallUnitChangeResponse { get; set; }
        public partial class GoodsCpsMallUnitChangeResponseResponseModel : PddResponseModel
        {
            /// <summary>
            /// 是否成功
            /// </summary>
            [JsonProperty("result")]
            public bool? Result { get; set; }

        }

    }

}
