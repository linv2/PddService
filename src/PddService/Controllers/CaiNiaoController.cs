using CaiNiaoSDK;
using CaiNiaoSDK.Models.DTO;
using CaiNiaoSDK.Models.Request;
using CaiNiaoSDK.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddService.Common;
using PddService.Common.Attribute;
using PddService.Common.Controller;
using PddService.Common.Result;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace PddService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CaiNiaoController : BaseController<CaiNiaoController>
    {
        private CaiNiaoClient caiNiaoClient;
        private IHttpClientFactory httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <param name="siteConfig"></param>
        /// <param name="caiNiaoClient"></param>
        /// <param name="httpClientFactory"></param>
        public CaiNiaoController(ILogger<CaiNiaoController> logger, PddDbContext dbContext, SiteConfig siteConfig,
            CaiNiaoClient caiNiaoClient, IHttpClientFactory httpClientFactory) : base(logger, dbContext, siteConfig)
        {
            this.caiNiaoClient = caiNiaoClient;
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 菜鸟OAuth授权
        /// </summary>
        /// <param name="url">回调地址</param>
        /// <param name="ext">扩展参数</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
        public IActionResult Authorize(string url = "/CaiNiao/Code", string ext = null)
        {
            string pddOAuthUrl = caiNiaoClient.GetOAuthUrl(SiteConfig.Url + url, ext);
            return Redirect(pddOAuthUrl);
        }

        /// <summary>
        /// 菜鸟OAuth授权回调函数
        /// </summary>
        /// <param name="accessCode">access code</param>
        /// <param name="ext">扩展参数</param>
        /// <param name="appkey">app key</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
        public IActionResult Code(string accessCode, string appkey, string ext="/bind")
        {
            var resultParam = string.Empty;
            var response = caiNiaoClient.AccessToken(accessCode);
            var accessTokenInfo = response?.AccessTokens?.FirstOrDefault();
            if (accessTokenInfo != null)
            {
                DbContext.User.Where(x => x.Id == UserInfo.Id).Update(x => new User
                {
                    CaiNiaoAccessToken = accessTokenInfo.AccessToken
                });
                DbContext.SaveChanges();
            }
            resultParam = "bind=cainiao";
            if (!string.IsNullOrEmpty(ext))
            {
                if (ext.IndexOf("?") > -1)
                {
                    ext += $"&{resultParam}";
                }
                else
                {
                    ext += $"?{resultParam}";
                }
            }

            return new RedirectResult(ext);
        }

        /// <summary>
        /// 获得菜鸟打印模板
        /// </summary>
        /// <param name="cpCode">快递编码</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Result<CloudPrintStandDardTemplatesResponse>), 200)]
        [HttpGet]
        public IActionResult GetClouPrintTemplates(string cpCode = null)
        {
            var response = caiNiaoClient.Request(new CloudPrintStandDardTemplatesRequest() { CpCode = cpCode },
                UserInfo.CaiNiaoAccessToken);

            return Success(response.Data);
        }

        /// <summary>
        /// 查询菜鸟快递账户信息
        /// </summary>
        /// <param name="cpCode">快递编码</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<WaybillBranchAccountDTO>), 200)]
        public IActionResult QueryWaybill(string cpCode = null)
        {
            var response = caiNiaoClient.Request(new TMSWaybillSubscriptionQueryRequest() { CpCode = cpCode },
                UserInfo.CaiNiaoAccessToken);
            if (!response.Success)
            {
                return Fail("获取菜鸟面单失败");
            }

            var branchAccount = response.WaybillApplySubscriptionCols.FirstOrDefault()?.BranchAccountCols
                .FirstOrDefault();
            return Success(branchAccount);
        }

        /// <summary>
        /// 查询菜鸟快递OpCode是否存在
        /// </summary>
        /// <param name="cpCode">快递编码</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<string>>), 200)]
        public IActionResult QueryOpCodeList(string cpCode = null)
        {
            var response = caiNiaoClient.Request(new TMSWaybillSubscriptionQueryRequest() { CpCode = cpCode },
                UserInfo.CaiNiaoAccessToken);
            if (!response.Success)
            {
                return Fail("获取菜鸟面单失败");
            }

            var opCodeList = response.WaybillApplySubscriptionCols.Select(x => x.cpCode);
            return Success(opCodeList);
        }

        /// <summary>
        /// 查询默认快递菜鸟账户信息
        /// </summary>
        /// <returns>快递编码</returns>
        [HttpGet,NoLogger]
        [ProducesResponseType(typeof(Result<WaybillBranchAccountDTO>), 200)]
        public IActionResult DefaultQueryWaybill()
        {
            var printTemplate = DbContext.PrintTemplate.Include(x => x.Express)
                .FirstOrDefault(x => x.Default && !x.Delete && x.UserId == UserInfo.Id);
            Assert.IsNull(printTemplate, "没有默认模板");
            return QueryWaybill(printTemplate?.Express.CaiNiaoResCode);
        }

        /// <summary>
        /// 读取菜鸟账号绑定状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CaiNiaoWaybillStatus()
        {
            var userId = UserInfo.Id;
            var result = DbContext.User.FirstOrDefault(x => x.Id == userId);
            return Success(new
            {
                CaiNiaoWaybill = !string.IsNullOrEmpty(result?.CaiNiaoAccessToken)
            });
        }
    }
}