using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddOpenSDK.Models;
using PddOpenSDK.Models.Request.Logistics;
using PddOpenSDK.Models.Request.Mall;
using PddService.Code;
using PddService.Code.Manage;
using PddService.Common;
using PddService.Common.Controller;
using PddService.Common.Result;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using Z.EntityFramework.Plus;
using static PddOpenSDK.Models.Response.Logistics.GetLogisticsCompaniesResponseModel;

namespace PddService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class OAuthController : BaseController<OAuthController>
    {
        private PddClient pddClient;
        private MallManage mallManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <param name="pddClient"></param>
        /// <param name="siteConfig"></param>
        /// <param name="mallManager"></param>
        public OAuthController(ILogger<OAuthController> logger, PddDbContext dbContext, PddClient pddClient, SiteConfig siteConfig, MallManage mallManager) : base(logger, dbContext, siteConfig)
        {
            this.pddClient = pddClient;
            this.mallManager = mallManager;

        }
        /// <summary>
        /// 拼多多OAuth授权
        /// </summary>
        /// <param name="url"></param>
        /// <param name="state"></param>
        /// <returns>重定向</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
        public IActionResult Authorize(string url = "/OAuth/Code", string state = null)
        {
            string pddOAuthUrl = pddClient.GetWebOAuthUrl(SiteConfig.Url + url, state);
            return Redirect(pddOAuthUrl);
        }
        /// <summary>
        /// 拼多多OAuth授权回调
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="state">state</param>
        /// <returns>重定向</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
        public IActionResult Code(string code, string state)
        {
            var resultParam = string.Empty;
            try
            {
                var response = pddClient.AccessToken(code, SiteConfig.Url + "/OAuth/Code", state);
                var mall = mallManager.UpdateMallMsgByAccessToken(UserInfo.Id, response.OwnerId, response.OwnerName, response.AccessToken, response.RefreshToken, response.ExpiresIn);
                resultParam = $"ownerName={WebUtility.UrlEncode(mall.MallName)}";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "拼多多Code获取AccessToken失败");
                resultParam = $"error={WebUtility.UrlEncode(ex.Message)}";
            }
            if (!string.IsNullOrEmpty(state))
            {
                if (state.IndexOf("?") > -1)
                {
                    state += $"&{resultParam}";
                }
                else
                {

                    state += $"?{resultParam}";
                }
            }
            return new RedirectResult(state);
        }
        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="mallId">店铺Id</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult Refresh(string mallId)
        {

            mallManager.UpdateAccessToken(mallId, UserInfo.Id);
            return OkResult;
        }
        /// <summary>
        /// 获取多多快递公司
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        [ProducesResponseType(typeof(Result<LogisticsCompaniesGetResponseResponseModel>), 200)]
        public IActionResult GetLogisticsCompanies()
        {
            var request = new GetLogisticsCompaniesRequestModel();
           var response= pddClient.Request(request);
            return Success(response.LogisticsCompaniesGetResponse);
        }
    }
}