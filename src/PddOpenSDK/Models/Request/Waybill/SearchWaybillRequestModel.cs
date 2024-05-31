using Newtonsoft.Json;
using PddOpenSDK.Attribute;
using PddOpenSDK.Models.Response.Waybill;

namespace PddOpenSDK.Models.Request.Waybill
{
    /// <summary>
    /// 查询面单服务订购及面单使用情况
    /// </summary>
    [PddRequestMethod("pdd.waybill.search")]
    public partial class SearchWaybillRequestModel : PddRequestModel<SearchWaybillResponseModel>
    {
        /// <summary>
        /// 物流公司code
        /// </summary>
        [JsonProperty("wp_code")]
        public string WpCode { get; set; }

    }

}
