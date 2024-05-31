using Newtonsoft.Json;
using PddOpenSDK.Attribute;
using PddOpenSDK.Models.Response.Waybill;

namespace PddOpenSDK.Models.Request.Waybill
{

    /// <summary>
    /// 获取所有标准电子面单模板
    /// </summary>
    [PddRequestMethod("pdd.cloudprint.stdtemplates.get")]
    public partial class GetCloudprintStdtemplatesRequestModel : PddRequestModel<GetCloudprintStdtemplatesResponseModel>
    {
        /// <summary>
        /// 快递公司code
        /// </summary>
        [JsonProperty("wp_code")]
        public string WpCode { get; set; }

    }

}
