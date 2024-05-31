using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.DTO
{
    /// <summary>
    /// 发货地址
    /// </summary>
    public class AddressDTO
    {

        /// <summary>
        /// 省
        /// </summary>
        [JsonProperty("province")]
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }



        /// <summary>
        /// 街道
        /// </summary>
        [JsonProperty("town")]
        public string Town { get; set; }


        /// <summary>
        /// 区县
        /// </summary>
        [JsonProperty("district")]
        public string District { get; set; }


        /// <summary>
        /// 详细地址
        /// </summary>
        [JsonProperty("detail")]
        public string Detail { get; set; }

    }
}
