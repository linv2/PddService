using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK
{
   public static  class PddUrlHelper
    {

        /// <summary>
        /// access_token
        /// </summary>
        public const  string TokenUrl = "https://open-api.pinduoduo.com/oauth/token";
        /// <summary>
        /// 商家授权地址
        /// </summary>
        public const  string MmsURL = "https://mms.pinduoduo.com/open.html";
        /// <summary>
        /// 移动端授权地址
        /// </summary>
        public const  string MaiURL = "https://mai.pinduoduo.com/h5-login.html";
        /// <summary>
        /// 多多客授权地址
        /// </summary>
        public const  string DDKUrl = "https://jinbao.pinduoduo.com/open.html";

        /// <summary>
        /// 获取网页授权地址
        /// </summary>
        /// <param name="pddClient"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetWebOAuthUrl(this PddClient pddClient,string callbackUrl, string state = null)
        {
            string url = MmsURL + "?response_type=code&client_id=" + pddClient.ClientId + "&redirect_uri=" + callbackUrl;
            if (!string.IsNullOrEmpty(state))
            {
                url += "&state=" + state;
            }
            return url;
        }

        /// <summary>
        /// 获取移动网页授权地址
        /// </summary>
        /// <param name="pddClient"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetH5OAuthUrl(this PddClient pddClient,string callbackUrl, string state = null)
        {
            string url = MaiURL + "?response_type=code&client_id=" + pddClient.ClientId + "&redirect_uri=" + callbackUrl + "&view=h5";
            if (!string.IsNullOrEmpty(state))
            {
                url += "&state=" + state;
            }
            return url;
        }
    }
}
