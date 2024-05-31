using System.Collections.Generic;
using Newtonsoft.Json;
namespace PddOpenSDK.Models.Request.Openmsg
{
    public partial class MsgOpenMsgServiceSendRequestModel : PddRequestModel
    {
        /// <summary>
        /// 接收短信的手机号码列表,["15900000000", "17600000000"]
        /// </summary>
        [JsonProperty("phone_numbers")]
        public List<string> PhoneNumbers { get; set; }
        /// <summary>
        /// 短信签名名称
        /// </summary>
        [JsonProperty("sign_name")]
        public string SignName { get; set; }
        /// <summary>
        /// 短信模板ID
        /// </summary>
        [JsonProperty("template_code")]
        public long TemplateCode { get; set; }
        /// <summary>
        /// 短信模板变量对应的实际值，JSON格式
        /// </summary>
        [JsonProperty("template_param")]
        public Dictionary<string, object> TemplateParam { get; set; }
        /// <summary>
        /// 业务请求唯一标识
        /// </summary>
        [JsonProperty("out_id")]
        public string OutId { get; set; }
        public partial class TemplateParamRequestModel : PddRequestModel
        {
            /// <summary>
            /// 模板变量名
            /// </summary>
            [JsonProperty("$key")]
            public string Key { get; set; }
            /// <summary>
            /// 模板变量值
            /// </summary>
            [JsonProperty("$value")]
            public string Value { get; set; }

        }

    }

}
