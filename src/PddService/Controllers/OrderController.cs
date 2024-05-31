using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PddService.Code.AsyncLogicHandler;
using PddService.Code.DynamicExpression;
using PddService.Code.Manage.Order;
using PddService.Common;
using PddService.Common.Controller;
using PddService.Common.Result;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using Serialize.Linq.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Z.EntityFramework.Plus;
using JsonSerializer = Serialize.Linq.Serializers.JsonSerializer;

namespace PddService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderController : BaseController<OrderController>
    {
        private OrderStatusSyncManage orderStatusSyncManage;
        private AsyncLogicManager asyncLogicManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <param name="siteConfig"></param>
        /// <param name="orderStatusSyncManage"></param>
        /// <param name="asyncLogicManager"></param>
        public OrderController(ILogger<OrderController> logger, PddDbContext dbContext, SiteConfig siteConfig, OrderStatusSyncManage orderStatusSyncManage, AsyncLogicManager asyncLogicManager) : base(logger, dbContext, siteConfig)
        {
            this.orderStatusSyncManage = orderStatusSyncManage;
            this.asyncLogicManager = asyncLogicManager;
        }
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页的数量</param>
        /// <param name="ownerName">店铺名字</param>
        /// <param name="orderSn">订单号</param>
        /// <param name="name">收货人名字</param>
        /// <param name="mobile">收货人电话</param>
        /// <param name="waybillStatus">发货状态</param>
        /// <param name="printStatus">打印状态</param>
        /// <param name="orderStatus">多多订单状态</param>
        /// <param name="valid">订单有效性</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<Order>>), 200)]
        public IActionResult List(int pageIndex = 1, int pageSize = 10, string ownerName = null, string orderSn = null, string name = null, string mobile = null, int waybillStatus = -1, int printStatus = -1, int orderStatus = -1, bool? valid = null,
            DateTime? startTime = null, DateTime? endTime = null)
        {
            pageIndex--;
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            var query = DbContext.Order
                .Include(x => x.Express)
                .Include(x => x.Mall)
                .Include(x => x.OrderDetail)
                .AsNoTracking();//.
                                // Where(x => x.ProductStatus == ProductStatus.Success);
                                //if (UserType == UserType.Agent) {
            query = query.Where(x => x.UserId == UserInfo.Id);

            if (!string.IsNullOrEmpty(ownerName))
            {
                var mallIdList = DbContext.Mall.Where(x => x.UserId == UserInfo.Id && x.MallName.Contains(ownerName)).Select(x => x.Id);
                query = query.Where(x => mallIdList.Contains(x.MallId));
            }
            if (!string.IsNullOrEmpty(orderSn))
            {
                query = query.Where(x => x.OrderSn.Equals(orderSn));
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.ReceiverName.Equals(name));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                query = query.Where(x => x.ReceiverPhone.Equals(name));
            }
            if (waybillStatus > -1)
            {
                query = query.Where(x => x.WaybillStatus.Equals(waybillStatus == 1));
            }
            if (printStatus > -1)
            {
                query = query.Where(x => x.PrintStatus.Equals(printStatus == 1));
            }
            if (orderStatus > 0)
            {
                query = query.Where(x => x.OrderStatus.Equals(orderStatus));
            }

            if (valid.HasValue && valid.Value)
            {
                query = query.Where(x => x.AfterSalesStatus == 0);
            }
            if (startTime.HasValue)
            {
                query = query.Where(x => x.ConfirmTime >= startTime);
            }
            if (endTime.HasValue)
            {
                query = query.Where(x => x.ConfirmTime < endTime);
            }

            var data = query.OrderByDescending(x => x.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return SuccessList(data, query.Count());
        }

        /// <summary>
        /// 根据Id读取订单
        /// </summary>
        /// <param name="id">订单Id</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<Order>), 200)]
        public IActionResult Get(int id)
        {
            var result = DbContext.Order
                   .Include(x => x.Express)
                   .Include(x => x.Mall)
                   .Include(x => x.OrderDetail)
                   .AsNoTracking()
                   .FirstOrDefault(x => x.UserId == UserInfo.Id && x.Id == id);
            return Success(result);
        }
        /// <summary>
        /// 同步订单状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult SyncOrderStatus([FromBody]int[] list)
        {
            orderStatusSyncManage.Sync(UserInfo.Id, list);
            return OkResult;
        }

        /// <summary>
        /// 已发货未通知平台的订单，通知平台发货成功
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult NotityPlatform([FromBody]int[] list)
        {
            var orderIdList = DbContext
                .Order
                .AsNoTracking()
                .Where(x => x.UserId == UserInfo.Id && list.Contains(x.Id) && x.WaybillStatus && !x.SyncStatus)
                .Select(x => x.Id)
                .ToArray();
            asyncLogicManager.Push(AsyncHandlerConstant.NotityPlatform, orderIdList);
            return SuccessMessage("任务已提交，请稍后刷新查看结果");
        }

        /// <summary>
        /// 单个订单发货
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public IActionResult SingleWaybill(int id)
        {
            var order = DbContext.Order.FirstOrDefault(x => x.UserId == UserInfo.Id && x.Id == id && !x.WaybillStatus);
            Assert.IsNull(order, "订单不存在或订单状态异常，请刷新列表重试");
            asyncLogicManager.Push(AsyncHandlerConstant.WaybillOrder, new[] { order.Id });
            return SuccessMessage("发货任务已提交，请稍后刷新查看结果");
        }
        /// <summary>
        /// 订单批量发货
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult BatchWaybill([FromBody]int[] list)
        {
            var orders = DbContext.Order.Where(x => x.UserId == UserInfo.Id && list.Contains(x.Id) && !x.WaybillStatus).ToArray();
            asyncLogicManager.Push(AsyncHandlerConstant.WaybillOrder, orders.Select(x => x.Id).ToArray());
            return SuccessMessage("任务已提交，请稍后刷新查看结果");
        }
        /// <summary>
        /// 修改订单打印状态
        /// </summary>
        /// <param name="waybillCodes"></param>
        /// <returns></returns>

        [HttpPost]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult UpdatePrintStatus([FromBody]string[] waybillCodes)
        {
            Assert.IsNull(waybillCodes, "提交数据为空");
            DbContext.Order.Where(x => !x.PrintStatus && x.UserId == UserInfo.Id && x.WaybillStatus && !x.PrintStatus && x.TrackingNumber != null && waybillCodes.Contains(x.TrackingNumber)).Update(x => new Order()
            {
                PrintStatus = true
            });
            DbContext.SaveChanges();
            return OkResult;
        }



        [HttpGet]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult Export(string ownerName = null, string orderSn = null, string name = null, string mobile = null, int waybillStatus = -1, int printStatus = -1, int orderStatus = -1, bool? valid = null,
           DateTime? startTime = null, DateTime? endTime = null)
        {
            var query = PredicateBuilder.True<Order>();
            if (!string.IsNullOrEmpty(ownerName))
            {
                var mallIdList = DbContext.Mall.Where(x => x.UserId == UserInfo.Id && x.MallName.Contains(ownerName)).Select(x => x.Id).ToArray();
                query = query.Where(x => mallIdList.Contains(x.MallId));
            }
            if (!string.IsNullOrEmpty(orderSn))
            {
                query = query.Where(x => x.OrderSn.Equals(orderSn));
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.ReceiverName.Equals(name));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                query = query.Where(x => x.ReceiverPhone.Equals(name));
            }
            if (waybillStatus > -1)
            {
                query = query.Where(x => x.WaybillStatus.Equals(waybillStatus == 1));
            }
            if (printStatus > -1)
            {
                query = query.Where(x => x.PrintStatus.Equals(printStatus == 1));
            }
            if (orderStatus > 0)
            {
                query = query.Where(x => x.OrderStatus.Equals(orderStatus));
            }

            if (valid.HasValue && valid.Value)
            {
                query = query.Where(x => x.AfterSalesStatus == 0);
            }
            if (startTime.HasValue)
            {
                query = query.Where(x => x.ConfirmTime >= startTime);
            }
            if (endTime.HasValue)
            {
                query = query.Where(x => x.ConfirmTime < endTime);
            }
            var serializer = new ExpressionSerializer(new JsonSerializer());
            string value = serializer.SerializeText(query);
            var hash = value.Sha1();
            if (DbContext.AsyncTask.Any(x => x.TaskParamHash == hash && x.TaskStatus==0))
            {
                return Fail("任务已经存在，请勿重复提交");
            }
            var model = new AsyncTask()
            {
                UserId = UserInfo.Id,
                TaskKey = hash + "_" + DateTime.Now.Ticks,
                TaskType = 1,
                CreatedTime = DateTime.Now,
                TaskParam = value,
                TaskParamHash=hash,
                TaskStatus = 0
            };
            DbContext.AsyncTask.Add(model);
            DbContext.SaveChanges();
            asyncLogicManager.Push(AsyncHandlerConstant.ExportOrder, model);
            return Result.Success(model.TaskKey, "订单数据生成中，请在任务列表中查看").ActionResult;
        }


    }
}
