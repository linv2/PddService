using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Stock
{
    public partial class GetExpressDepotInfoRequestModel : PddRequestModel
    {
        /// <summary>
        /// 仓库id
        /// </summary>
        [JsonProperty("depot_id")]
        public long DepotId { get; set; }

    }

}
