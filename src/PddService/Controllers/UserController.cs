using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PddOpenSDK;
using PddOpenSDK.Models.Request.Order;
using PddOpenSDK.Models.Request.Waybill;
using PddService.Common;
using PddService.Common.Controller;
using PddService.Common.Filters;
using PddService.Common.Result;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using Z.EntityFramework.Plus;

namespace PddService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UserController : BaseController<UserController>
    {
        private PddClient pddClient;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <param name="siteConfig"></param>
        /// <param name="pddClient"></param>
        public UserController(ILogger<UserController> logger, PddDbContext dbContext, SiteConfig siteConfig, PddClient pddClient) : base(logger, dbContext, siteConfig)
        {
            this.pddClient = pddClient;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        [HttpPost, ModelValidation, AllowAnonymous]
        [ProducesResponseType(typeof(Result<User>), 200)]
        public IActionResult Login([FromBody]JObject jObject)
        {
            var userName = jObject["userName"].Value<string>();
            var passWord = jObject["passWord"].Value<string>();
            Assert.IsNullOrEmpty(userName, "用户名不能为空");
            Assert.IsNullOrEmpty(userName, "密码不能为空");
            var result = DbContext.User.FirstOrDefault(x => x.UserName == userName && x.PassWord == passWord);
            Assert.IsNull(result, "用户名或密码错误");
            Assert.IsTrue(result.Disable, "账号已被禁用");
            HttpContext.Session.SetInt32("userId", result.Id);
            result.LoginCount++;
            result.LastLoginTime = DateTime.Now;
            result.LastLoginIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            DbContext.SaveChanges();
            return Success(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Result<User>), 200)]
        public IActionResult Info()
        {
            return Success(UserInfo);
        }

        /// <summary>
        /// 修改DisplayName
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, ModelValidation]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult UpdateDisplayName([FromBody,Bind("DisplayName")]User user)
        {
            DbContext.User.Where(x => x.Id == UserInfo.Id).Update(x => new User()
            {
                DisplayName = user.DisplayName
            });
            DbContext.SaveChanges();
            return OkResult;
        }
        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult UpdatePassword([FromBody]JObject jObject)
        {
            var oldPassword = jObject?["oldPassword"]?.Value<string>();
            var newPassword = jObject?["newPassword"]?.Value<string>();
            Assert.IsNullOrEmpty(oldPassword, "旧密码不能为空");
            Assert.IsNullOrEmpty(newPassword, "新密码不能为空");
            if (oldPassword != UserInfo.PassWord)
            {
                return Fail("旧密码错误");
            }

            DbContext.User.Where(x => x.Id == UserInfo.Id).Update(x => new User()
            {
                PassWord = newPassword
            });
            DbContext.SaveChanges();
            return OkResult;
        }

        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LogOut(string url = null)
        {
            HttpContext.Session.Clear();
            return new RedirectResult(url ?? "/");
        }
        [HttpGet, AllowAnonymous]
        public IActionResult Test()
        {
            HttpContext.Session.SetInt32("userId", 1);
            return Success(DbContext.User.FirstOrDefault());
        }

       



    }
}