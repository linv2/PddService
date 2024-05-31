using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Pmc
{
    public partial class GetPmcUserRequestModel : PddRequestModel
    {
        /// <summary>
        /// 用户唯一id
        /// </summary>
        [JsonProperty("owner_id")]
        public string OwnerId { get; set; }

    }

}
