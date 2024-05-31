using CaiNiaoSDK;
using CaiNiaoSDK.Models.DTO;
using CaiNiaoSDK.Models.Request;
using CaiNiaoSDK.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PddOpenSDK;
using PddOpenSDK.Models.Request.Logistics;
using PddOpenSDK.Models.Request.Order;
using PddService.Common;
using PddService.Common.Exceptions;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace PddService.Code.Manage.Order
{
    public class OrderWaybillManage
    {
        private ILogger<OrderWaybillManage> logger;
        private PddDbContext dbContext;
        private CaiNiaoClient caiNiaoClient;
        private PddClient pddClient;
        private ITokenContainer tokenContainer;
        private SiteConfig siteConfig;
        public OrderWaybillManage(ILogger<OrderWaybillManage> logger, PddDbContext dbContext, CaiNiaoClient caiNiaoClient, ITokenContainer tokenContainer, SiteConfig siteConfig, PddClient pddClient)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.caiNiaoClient = caiNiaoClient;
            this.tokenContainer = tokenContainer;
            this.siteConfig = siteConfig;
            this.pddClient = pddClient;
        }

        public bool OrderWaybill(int mallId, params DataAccess.Entity.Order[] orders)
        {
            if (orders.Count() > 10)
            {
                logger.LogInformation("请求面单列表（上限10个）");
                return false;
            }
            if (orders.Count() == 0)
            {
                logger.LogInformation("没有可发货订单");
                return false;
            }
            var ordersId = orders.Select(x => x.Id);
            var orderDict = orders.ToDictionary(x => x.Id);
            var userInfo = dbContext.User.FirstOrDefault(x => x.Id == orders.FirstOrDefault().UserId);
            var printTemplate = GetPrintTemplate(mallId);
            var mallInfo = tokenContainer.GetMall(mallId);


            var orderRequest = BuildRequestParam(mallInfo, printTemplate, orders.ToList());
            TMSWaybillGetResponse response = null;
            try
            {
                response = caiNiaoClient.Request(orderRequest, userInfo.CaiNiaoAccessToken);
                if (response.Success)
                {
                    logger.LogInformation($"订单ID={string.Join(',', ordersId)}的订单，菜鸟面单创建成功，开始保存面单信息和打印数据");
                    SaveWaybill(response.WaybillCloudPrintResponseList, printTemplate);
                    logger.LogInformation($"订单ID={string.Join(',', ordersId)}的订单，菜鸟面单保存完成，开始通知平台发货");
                    NotityPlatform(response.WaybillCloudPrintResponseList, mallInfo, orderDict, printTemplate);

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"订单ID={string.Join(',', ordersId)}的订单，菜鸟创建面单失败");
            }
            return response?.Success ?? false;
        }
        public bool OrderWaybill(int mallId, params int[] ordersId)
        {
            var orders = dbContext.Order.Include(x => x.OrderDetail).Where(x => ordersId.Contains(x.Id) && x.MallId == mallId && x.AfterSalesStatus == 0 && x.SelfOrderStatus == SelfOrderStatus.Created && x.OrderStatus == 1).ToList();
            if (orders.Count > 10)
            {
                logger.LogInformation("请求面单列表（上限10个）");
                return false;
            }
            if (orders.Count == 0)
            {
                logger.LogInformation("没有可发货订单");
                return false;
            }
            var orderDict = orders.ToDictionary(x => x.Id);
            var userInfo = dbContext.User.FirstOrDefault(x => x.Id == orders.FirstOrDefault().UserId);
            var printTemplate = GetPrintTemplate(mallId);
            var mallInfo = tokenContainer.GetMall(mallId);


            var orderRequest = BuildRequestParam(mallInfo, printTemplate, orders);
            TMSWaybillGetResponse response=null;
            try
            {
                response = caiNiaoClient.Request(orderRequest, userInfo.CaiNiaoAccessToken);
                if (response.Success)
                {
                    logger.LogInformation($"订单ID={string.Join(',', ordersId)}的订单，菜鸟面单创建成功，开始保存面单信息和打印数据");
                    SaveWaybill(response.WaybillCloudPrintResponseList, printTemplate);
                    logger.LogInformation($"订单ID={string.Join(',', ordersId)}的订单，菜鸟面单保存完成，开始通知平台发货");
                    NotityPlatform(response.WaybillCloudPrintResponseList, mallInfo, orderDict,printTemplate);

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"订单ID={string.Join(',', ordersId)}的订单，菜鸟创建面单失败");
            }
            return response?.Success ?? false;
        }

        private void SaveWaybill(List<WaybillCloudPrintDTO> wayBillCloudPrintResponseList, PrintTemplate printTemplate)
        {
            try
            {
                foreach (var item in wayBillCloudPrintResponseList)
                {
                    var orderId = Convert.ToInt32(item.ObjectId);
                    dbContext.Order.Where(x => x.Id == orderId).Update(x => new DataAccess.Entity.Order()
                    {
                        ExpressId = printTemplate.ExpressId,
                        TrackingNumber = item.WaybillCode,
                        WaybillStatus = true,
                        SelfOrderStatus=SelfOrderStatus.Waybill,
                        PrintData = item.PrintData
                    });
                }
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"面单信息和打印数据保存失败，保存数据={JsonConvert.SerializeObject(wayBillCloudPrintResponseList)}");
            }
        }
        private void NotityPlatform(List<WaybillCloudPrintDTO> wayBillCloudPrintResponseList, Mall mallInfo, Dictionary<int, DataAccess.Entity.Order> orderDict, PrintTemplate printTemplate)
        {
            var list = new List<int>();
            foreach (var item in wayBillCloudPrintResponseList)
            {
                var orderId = Convert.ToInt32(item.ObjectId);
                var order = orderDict[orderId];
                try
                {
                    var request = new SyncErpOrderRequestModel()
                    {
                        OrderSn = order.OrderSn,
                        OrderState=1,
                        WaybillNo= item.WaybillCode,
                        LogisticsId= printTemplate.Express.PddLogisticsId
                    };
                    var pddResponse = pddClient.Request(request, mallInfo.AccessToken);
                    if (pddResponse.Success??false)
                    {
                        list.Add(orderId);
                    }
                    else
                    {
                        logger.LogError($"订单{order.OrderSn}通知平台发货失败，错误代码={pddResponse.ErrorCode}，错误描述={pddResponse.ErrorMsg}");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"订单={order.OrderSn}通知平台发货异常，快递号={item.WaybillCode}");
                }
            }

            try
            {
                dbContext.Order.Where(x => list.Contains(x.Id)).Update(x => new DataAccess.Entity.Order()
                {
                    SyncStatus = true,
                    SyncTime = DateTime.Now
                });
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"订单已经成功发货，同步字段更新异常 order_id={string.Join(',', list)}");
            }
        }

        private TMSWaybillGetRequest BuildRequestParam(Mall mallInfo, PrintTemplate printTemplate, List<DataAccess.Entity.Order> orders)
        {
            var waybillAdds = GetAddress(mallInfo.Id);
            var orderRequest = new TMSWaybillGetRequest()
            {
                CpCode = printTemplate.Express.CaiNiaoCode,
                Sender = new UserInfoDTO()
                {
                    Name = waybillAdds.Name,
                    Mobile = waybillAdds.Mobile,
                    Address = new AddressDTO()
                    {
                        Province = waybillAdds.Province,
                        City = waybillAdds.City,
                        Town = waybillAdds.Town,
                        Detail = waybillAdds.Detail
                    }
                },
                TradeOrderInfoDtos = new List<TradeOrderInfoDTO>()
            };
            foreach (var order in orders)
            {
                var tradeOrderInfoDTO = new TradeOrderInfoDTO()
                {
                    ObjectId = Convert.ToString(order.Id),
                    OrderInfo = new OrderInfoDTO()
                    {
                        OrderChannelsType = "PIN_DUO_DUO",//来源拼多多 https://support-cnkuaidi.taobao.com/doc.htm#?docId=105085&docType=1
                        TradeOrderList = new List<string>() { order.OrderSn }
                    },
                    PackageInfo = new PackageInfoDTO()
                    {
                        Id = Convert.ToString(order.Id),
                        Items = order.OrderDetail.Select(x => new Item()
                        {
                            Name = x.GoodsName,
                            Count = x.GoodsCount
                        }).ToList()
                    },
                    Recipient = new UserInfoDTO()
                    {
                        Name = order.ReceiverName,
                        Mobile = order.ReceiverPhone,
                        Address = new AddressDTO()
                        {
                            Province = order.Province,
                            City = order.City,
                            Town = order.Town,
                            Detail = order.Address
                        }
                    },
                    UserId = mallInfo.Id,
                    TemplateUrl = printTemplate.SourceUrl
                };
                orderRequest.TradeOrderInfoDtos.Add(tradeOrderInfoDTO);
            }
            return orderRequest;
        }
        private Address GetAddress(int mallId)
        {
            var mallInfo = tokenContainer.GetMall(mallId);
            if (mallInfo.AddressId.HasValue)
            {
                return dbContext.Address.FirstOrDefault(x => x.Id == mallInfo.AddressId.Value);
            }
            else
            {
                return dbContext.Address.FirstOrDefault(x => x.UserId == mallInfo.UserId && x.Default);
            }
        }
        private PrintTemplate GetPrintTemplate(int mallId)
        {

            var mallInfo = tokenContainer.GetMall(mallId);
            if (mallInfo.PrintTemplateId.HasValue)
            {
                return dbContext.PrintTemplate.Include(x => x.Express).FirstOrDefault(x => x.Id == mallInfo.PrintTemplateId.Value);
            }
            else
            {
                return dbContext.PrintTemplate.Include(x => x.Express).FirstOrDefault(x => x.UserId == mallInfo.UserId && x.Default);
            }
        }
    }
}
