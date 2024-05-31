using System.Collections.Generic;
using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Ddktools
{
    public partial class GenerateDdkOauthThemePromUrlResponseModel : PddResponseModel
    {
        /// <summary>
        /// 主题活动推广链接返回对象
        /// </summary>
        [JsonProperty("theme_promotion_url_generate_response")]
        public ThemePromotionUrlGenerateResponseResponseModel ThemePromotionUrlGenerateResponse { get; set; }
        public partial class ThemePromotionUrlGenerateResponseResponseModel : PddResponseModel
        {
            /// <summary>
            /// url_list
            /// </summary>
            [JsonProperty("url_list")]
            public List<UrlListResponseModel> UrlList { get; set; }
            public partial class UrlListResponseModel : PddResponseModel
            {
                /// <summary>
                /// 主题活动推广链接
                /// </summary>
                [JsonProperty("url")]
                public string Url { get; set; }
                /// <summary>
                /// 主题活动推广短链接
                /// </summary>
                [JsonProperty("short_url")]
                public string ShortUrl { get; set; }
                /// <summary>
                /// 主题活动推广移动链接
                /// </summary>
                [JsonProperty("mobile_url")]
                public string MobileUrl { get; set; }
                /// <summary>
                /// 主题活动推广移动短链接
                /// </summary>
                [JsonProperty("mobile_short_url")]
                public string MobileShortUrl { get; set; }
                /// <summary>
                /// 主题活动推广多人团链接
                /// </summary>
                [JsonProperty("multi_group_url")]
                public string MultiGroupUrl { get; set; }
                /// <summary>
                /// 主题活动推广多人团短链接
                /// </summary>
                [JsonProperty("multi_group_short_url")]
                public string MultiGroupShortUrl { get; set; }
                /// <summary>
                /// 主题活动推广多人团移动链接
                /// </summary>
                [JsonProperty("multi_group_mobile_url")]
                public string MultiGroupMobileUrl { get; set; }
                /// <summary>
                /// 主题活动推广多人团移动短链接
                /// </summary>
                [JsonProperty("multi_group_mobile_short_url")]
                public string MultiGroupMobileShortUrl { get; set; }
                /// <summary>
                /// 小程序信息
                /// </summary>
                [JsonProperty("we_app_info")]
                public WeAppInfoResponseModel WeAppInfo { get; set; }
                /// <summary>
                /// schema的链接
                /// </summary>
                [JsonProperty("schema_url")]
                public string SchemaUrl { get; set; }
                /// <summary>
                /// qq小程序信息
                /// </summary>
                [JsonProperty("qq_app_info")]
                public QqAppInfoResponseModel QqAppInfo { get; set; }
                public partial class WeAppInfoResponseModel : PddResponseModel
                {
                    /// <summary>
                    /// 小程序图片
                    /// </summary>
                    [JsonProperty("we_app_icon_url")]
                    public string WeAppIconUrl { get; set; }
                    /// <summary>
                    /// Banner图
                    /// </summary>
                    [JsonProperty("banner_url")]
                    public string BannerUrl { get; set; }
                    /// <summary>
                    /// 描述
                    /// </summary>
                    [JsonProperty("desc")]
                    public string Desc { get; set; }
                    /// <summary>
                    /// 来源名
                    /// </summary>
                    [JsonProperty("source_display_name")]
                    public string SourceDisplayName { get; set; }
                    /// <summary>
                    /// 小程序path值
                    /// </summary>
                    [JsonProperty("page_path")]
                    public string PagePath { get; set; }
                    /// <summary>
                    /// 用户名
                    /// </summary>
                    [JsonProperty("user_name")]
                    public string UserName { get; set; }
                    /// <summary>
                    /// 小程序标题
                    /// </summary>
                    [JsonProperty("title")]
                    public string Title { get; set; }
                    /// <summary>
                    /// 拼多多小程序id
                    /// </summary>
                    [JsonProperty("app_id")]
                    public string AppId { get; set; }

                }
                public partial class QqAppInfoResponseModel : PddResponseModel
                {
                    /// <summary>
                    /// 拼多多小程序id
                    /// </summary>
                    [JsonProperty("app_id")]
                    public string AppId { get; set; }
                    /// <summary>
                    /// Banner图
                    /// </summary>
                    [JsonProperty("banner_url")]
                    public string BannerUrl { get; set; }
                    /// <summary>
                    /// 描述
                    /// </summary>
                    [JsonProperty("desc")]
                    public string Desc { get; set; }
                    /// <summary>
                    /// 小程序path值
                    /// </summary>
                    [JsonProperty("page_path")]
                    public string PagePath { get; set; }
                    /// <summary>
                    /// 小程序icon
                    /// </summary>
                    [JsonProperty("qq_app_icon_url")]
                    public string QqAppIconUrl { get; set; }
                    /// <summary>
                    /// 来源名
                    /// </summary>
                    [JsonProperty("source_display_name")]
                    public string SourceDisplayName { get; set; }
                    /// <summary>
                    /// 小程序标题
                    /// </summary>
                    [JsonProperty("title")]
                    public string Title { get; set; }
                    /// <summary>
                    /// 用户名
                    /// </summary>
                    [JsonProperty("user_name")]
                    public string UserName { get; set; }

                }

            }

        }

    }

}
