using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PddService.Code;
using PddService.Code.Manage.Order;
using PddService.Common;
using PddService.Common.Controller;
using PddService.Common.Result;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using Z.EntityFramework.Plus;

namespace PddService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MallController : BaseController<MallController>
    {
        private OrderSyncManage orderSyncManage;
        private ITokenContainer tokenContainer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <param name="siteConfig"></param>
        /// <param name="orderSyncManage"></param>
        /// <param name="tokenContainer"></param>
        public MallController(ILogger<MallController> logger, PddDbContext dbContext, SiteConfig siteConfig, OrderSyncManage orderSyncManage, ITokenContainer tokenContainer) : base(logger, dbContext, siteConfig)
        {
            this.orderSyncManage = orderSyncManage;
            this.tokenContainer = tokenContainer;
        }
        /// <summary>
        /// 获取已绑定的店铺信息
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页的数量</param>
        /// <param name="mallName">店铺名字</param>
        /// <param name="ownerId">owner Id</param>
        /// <param name="ownerName">owner Name</param>
        /// <param name="valid">店铺是否还在有效期</param>
        /// <returns></returns>
        [HttpGet]

        [ProducesResponseType(typeof(Result<IEnumerable<Mall>>), 200)]
        public IActionResult List(int pageIndex = 1, int pageSize = 10, string mallName = null, string ownerId = null, string ownerName = null, int valid = -1)
        {
            pageIndex--;
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            var query = DbContext.Mall
                .AsNoTracking();//.
                                // Where(x => x.ProductStatus == ProductStatus.Success);
                                //if (UserType == UserType.Agent) {
            query = query.Where(x => x.UserId == UserInfo.Id);

            if (!string.IsNullOrEmpty(mallName))
            {
                query = query.Where(x => x.MallName.Contains(mallName));
            }
            if (!string.IsNullOrEmpty(ownerId))
            {
                query = query.Where(x => x.OwnerId.Contains(ownerId));
            }
            if (!string.IsNullOrEmpty(ownerName))
            {
                query = query.Where(x => x.OwnerName.Contains(ownerName));
            }
            if (valid.Equals(0))
            {
                query = query.Where(x => x.ExpireTime.HasValue && x.ExpireTime.Value < DateTime.Now);
            }
            if (valid.Equals(1))
            {
                query = query.Where(x => x.ExpireTime.HasValue && x.ExpireTime.Value > DateTime.Now);
            }

            var data = query.OrderByDescending(x => x.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return SuccessList(data, query.Count());
        }

        /// <summary>
        /// 根据Id获取店铺
        /// </summary>
        /// <param name="Id">店铺Id</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<Mall>), 200)]
        public IActionResult Get(int Id)
        {
            var mallInfo = DbContext.Mall.FirstOrDefault(x => x.Id == Id && x.UserId == UserInfo.Id);
            return Success(mallInfo);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult UpdateConfig([FromBody] JObject jObject)
        {
            var mallId = jObject["id"].Value<int>();

            var strAddressId = jObject["addressId"].Value<string>();
            var strPrintTemplateId = jObject["printTemplateId"].Value<string>();
            var autoSendOut = jObject["autoSendOut"].Value<bool>();
            var autoCancel = jObject["autoCancel"].Value<bool>();
            var autoNotity = jObject["autoNotity"].Value<bool>();
            var _autoSendOutTimeRange = jObject["autoSendOutTimeRange"].Values<int>();


            var autoSendOutStartTime = _autoSendOutTimeRange.ElementAt(0);
            var autoSendOutEndTime = _autoSendOutTimeRange.ElementAt(1);


            int? addressId = null, printTemplateId = null;
            if (!string.IsNullOrEmpty(strAddressId))
            {
                Assert.IsNumber(strAddressId);
                addressId = Convert.ToInt32(strAddressId);
                Assert.IsFalse(DbContext.Address.Any(x => x.UserId == UserInfo.Id && x.Id == addressId.Value), "参数错误");
            }
            if (!string.IsNullOrEmpty(strPrintTemplateId))
            {
                Assert.IsNumber(strPrintTemplateId);
                printTemplateId = Convert.ToInt32(strPrintTemplateId);
                Assert.IsFalse(DbContext.PrintTemplate.Any(x => x.UserId == UserInfo.Id && x.Id == printTemplateId.Value), "参数错误");
            }
            DbContext.Mall.Where(x => x.Id == mallId && x.UserId == UserInfo.Id).Update(x => new Mall()
            {
                AddressId = addressId,
                PrintTemplateId = printTemplateId,
                AutoSendOut = autoSendOut,
                AutoCancel = autoCancel,
                AutoNotity= autoNotity,
                AutoSendOutStartTime = autoSendOutStartTime,
                AutoSendOutEndTime = autoSendOutEndTime
            });
            DbContext.SaveChanges();
            return OkResult;
        }

        /// <summary>
        /// 同步拼多多的订单
        /// </summary>
        /// <param name="id">店铺Id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult SyncOrder(int id, DateTime? startTime = null, DateTime? endTime = null)
        {
            var mallInfo = tokenContainer.GetMall(id);
            Assert.IsNotEquals(mallInfo.UserId, UserInfo.Id, "系统错误");
            var res = orderSyncManage.Sync(id, startTime, endTime);
            return SuccessMessage(res.ToString());
        }
        /// <summary>
        /// 批量同步拼多多订单
        /// </summary>
        /// <param name="mallIds">店铺Id列表“，”号分割</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult BatchSyncOrder(string mallIds, DateTime? startTime = null, DateTime? endTime = null)
        {
            var ids = mallIds.Split(',').Select(x => Convert.ToInt32(x));
            var collectModel = new CollectModel();
            foreach (var id in ids)
            {

                var mallInfo = tokenContainer.GetMall(id);
                if (mallInfo != null && mallInfo.UserId == UserInfo.Id)
                {
                    orderSyncManage.Sync(id, startTime, endTime, collectModel);
                }
            }
            return SuccessMessage(collectModel.ToString());
        }
    }
}