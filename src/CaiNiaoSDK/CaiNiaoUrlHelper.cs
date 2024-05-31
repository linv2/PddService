using CaiNiaoSDK;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK
{
   public static  class CaiNiaoUrlHelper
    {
        public const string OAuthUrl = "http://lcp.cloud.cainiao.com/permission/isv/grantpage.do?isvAppKey={0}&ext=&redirectUrl={1}&ext={2}";
        public const string TokenUrl = "http://lcp.cloud.cainiao.com/api/permission/exchangeToken.do?accessCode={0}&isvAppKey={1}&sign={2}";

        /// <summary>
        /// 获取网页授权地址
        /// </summary>
        /// <param name="pddClient"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string GetOAuthUrl(this CaiNiaoClient pddClient,string callbackUrl,string ext = null)
        {
           
            return string.Format(OAuthUrl,pddClient.AppKey,callbackUrl, ext);
        }

    }
}
