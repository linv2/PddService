using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Ad
{
    public partial class UpdateAdPlanDiscountResponseModel : PddResponseModel
    {
        /// <summary>
        /// true or false
        /// </summary>
        [JsonProperty("open_api_response")]
        public bool? OpenApiResponse { get; set; }

    }

}
