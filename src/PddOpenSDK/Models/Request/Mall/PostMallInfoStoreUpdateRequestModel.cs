using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Mall
{
    public partial class PostMallInfoStoreUpdateRequestModel : PddRequestModel
    {
        /// <summary>
        /// 门店营业状态
        /// </summary>
        [JsonProperty("business_status")]
        public int BusinessStatus { get; set; }
        /// <summary>
        /// 营业天数
        /// </summary>
        [JsonProperty("business_week_list")]
        public int BusinessWeekList { get; set; }
        /// <summary>
        /// 省市区三级地址-市
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }
        /// <summary>
        /// 省市区三级地址-区
        /// </summary>
        [JsonProperty("district")]
        public string District { get; set; }
        /// <summary>
        /// 结束营业时间段
        /// </summary>
        [JsonProperty("end_business_hour")]
        public string EndBusinessHour { get; set; }
        /// <summary>
        /// 腾讯地图POI信息ID
        /// </summary>
        [JsonProperty("poi_id")]
        public string PoiId { get; set; }
        /// <summary>
        /// 门店纬度
        /// </summary>
        [JsonProperty("poi_latitude")]
        public double PoiLatitude { get; set; }
        /// <summary>
        /// 门店经度
        /// </summary>
        [JsonProperty("poi_longitude")]
        public double PoiLongitude { get; set; }
        /// <summary>
        /// 省市区三级地址-省
        /// </summary>
        [JsonProperty("province")]
        public string Province { get; set; }
        /// <summary>
        /// 起始营业时间段
        /// </summary>
        [JsonProperty("start_business_hour")]
        public string StartBusinessHour { get; set; }
        /// <summary>
        /// 门店地址
        /// </summary>
        [JsonProperty("store_address")]
        public string StoreAddress { get; set; }
        /// <summary>
        /// 门店ID
        /// </summary>
        [JsonProperty("store_id")]
        public long StoreId { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        [JsonProperty("store_name")]
        public string StoreName { get; set; }
        /// <summary>
        /// 门店自有编号
        /// </summary>
        [JsonProperty("store_number")]
        public string StoreNumber { get; set; }
        /// <summary>
        /// 门店电话
        /// </summary>
        [JsonProperty("store_phone")]
        public string StorePhone { get; set; }

    }

}
