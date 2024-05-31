using CaiNiaoSDK;
using CaiNiaoSDK.Models.DTO;
using CaiNiaoSDK.Models.Request;
using CaiNiaoSDK.Models.Response;
using log4net.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PddOpenSDK;
using PddService.Code.AsyncLogicHandler;
using PddService.Code.AsyncLogicHandler.Model;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace PddService.Code.AsyncLogicHandler.Handlers
{
    public class AsyncWayBillHandler : AbsAsyncLoginHandlerBase<int[]>
    {
        private CaiNiaoClient caiNiaoClient;
        private AsyncLogicManager asyncLogicManager;
        public AsyncWayBillHandler(ILogger<AsyncWayBillHandler> logger, PddDbContext dbContext, PddClient pddClient, ITokenContainer tokenContainer, CaiNiaoClient caiNiaoClient, AsyncLogicManager asyncLogicManager) : base(logger, dbContext, pddClient, tokenContainer)
        {
            this.caiNiaoClient = caiNiaoClient;
            this.asyncLogicManager = asyncLogicManager;
        }

        public override string MessageTypeName => AsyncHandlerConstant.WaybillOrder.ToString();

        public override async Task Execute(int[] orderIds)
        {
            var orders = DbContext
                .Order
                .AsNoTracking()
                .Include(x=>x.OrderDetail)
                .Where(x => orderIds.Contains(x.Id) && !x.WaybillStatus)
                .ToArray();
            var orderLoopup = orders.ToLookup(x => x.MallId);
            var list = new List<int>();
            foreach (var item in orderLoopup)
            {
                var mallInfo = DbContext.Mall.AsNoTracking().Include(x => x.User).FirstOrDefault(x => x.Id == item.Key);
                OrderWaybill(mallInfo, item.ToArray());
                if (mallInfo.AutoNotity)
                {
                    list.AddRange(item.Select(x => x.Id));
                }
            }

           await asyncLogicManager.PushAsync(AsyncHandlerConstant.NotityPlatform, list.ToArray());

        }

        private void OrderWaybill(Mall mallInfo, Order[] orders)
        {
            if (!orders.Any())
            {
                return;
            }
            var printTemplate = GetPrintTemplate(mallInfo);
            var address = GetAddress(mallInfo);
            var orderRequest = BuildRequestParam(mallInfo, printTemplate, address, orders);
            TMSWaybillGetResponse response = null;
            try
            {
                response = caiNiaoClient.Request(orderRequest, mallInfo.User.CaiNiaoAccessToken);
                if (response.Success)
                {
                    SaveWaybill(response.WaybillCloudPrintResponseList, printTemplate);
                }
                else
                {
                    Logger.LogError($"订单ID={string.Join(',', orders.Select(x => x.Id))}的订单，菜鸟创建面单失败 ErrorCode={response.ErrorCode} ErrorMessage={response.ErrorMsg}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"订单ID={string.Join(',', orders.Select(x => x.Id))}的订单，菜鸟创建面单失败");
            }
        }



        private void SaveWaybill(List<WaybillCloudPrintDTO> wayBillCloudPrintResponseList, PrintTemplate printTemplate)
        {
            try
            {
                foreach (var item in wayBillCloudPrintResponseList)
                {
                    var orderId = Convert.ToInt32(item.ObjectId);
                    DbContext.Order.Where(x => x.Id == orderId).Update(x => new DataAccess.Entity.Order()
                    {
                        ExpressId = printTemplate.ExpressId,
                        TrackingNumber = item.WaybillCode,
                        WaybillStatus = true,
                        SelfOrderStatus = SelfOrderStatus.Waybill,
                        PrintData = item.PrintData
                    });
                }
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"面单信息和打印数据保存失败，保存数据={JsonConvert.SerializeObject(wayBillCloudPrintResponseList)}");
            }
        }

        /// <summary>
        /// 构建请求参数
        /// </summary>
        /// <param name="mallInfo"></param>
        /// <param name="printTemplate"></param>
        /// <param name="waybillAdds"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        private TMSWaybillGetRequest BuildRequestParam(Mall mallInfo, PrintTemplate printTemplate, Address waybillAdds, params Order[] orders)
        {
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


        private Address GetAddress(Mall mallInfo)
        {
            if (mallInfo.AddressId.HasValue)
            {
                return DbContext.Address.AsNoTracking().FirstOrDefault(x => x.Id == mallInfo.AddressId.Value);
            }
            else
            {
                return DbContext.Address.AsNoTracking().FirstOrDefault(x => x.UserId == mallInfo.UserId && x.Default);
            }
        }
        private PrintTemplate GetPrintTemplate(Mall mallInfo)
        {

            if (mallInfo.PrintTemplateId.HasValue)
            {
                return DbContext.PrintTemplate.AsNoTracking().Include(x => x.Express).FirstOrDefault(x => x.Id == mallInfo.PrintTemplateId.Value);
            }
            else
            {
                return DbContext.PrintTemplate.AsNoTracking().Include(x => x.Express).FirstOrDefault(x => x.UserId == mallInfo.UserId && x.Default);
            }
        }

    }
}
