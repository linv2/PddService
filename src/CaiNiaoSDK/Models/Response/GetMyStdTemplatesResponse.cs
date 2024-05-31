using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.Response
{
   public class GetMyStdTemplatesResponse: CaiNiaoResponseModel
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty("error_code")]
        public new string ErrorCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty("error_message")]
        public new string ErrorMsg { get; set; }

        [JsonProperty("data")]
        public List<UserTemplateResult> Data { get; set; }
    }
    public class UserTemplateResult
    {
        [JsonProperty("cp_code")]
        public string cpCode { get; set; }
        [JsonProperty("user_std_templates")]
        public List<UserTemplateDo> UserStdTemplates { get; set; }
    }
    public class UserTemplateDo
    {
        [JsonProperty("keys")]
        public List<KeyResult> Keys { get; set; }
        [JsonProperty("user_std_template_url")]
        public string UserStdTemplateUrl { get; set; }
        [JsonProperty("user_std_template_id")]
        public long UserStdTemplateId { get; set; }
        [JsonProperty("user_std_template_name")]
        public string UserStdTemplateName { get; set; }
    }
    public class KeyResult
    {
        [JsonProperty("key_name")]
        public string KeyName { get; set; }
    }
}
