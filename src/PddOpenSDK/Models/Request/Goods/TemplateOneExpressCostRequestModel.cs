using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Goods
{
    public partial class TemplateOneExpressCostRequestModel : PddRequestModel
    {
        /// <summary>
        /// 运费模板id
        /// </summary>
        [JsonProperty("cost_template_id")]
        public long CostTemplateId { get; set; }

    }

}
