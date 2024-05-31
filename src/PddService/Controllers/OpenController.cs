using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddOpenSDK.Models.Request.Goods;
using PddService.Common;
using PddService.Common.Controller;
using PddService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Controllers
{
    public class OpenController : BaseController<OpenController>
    {
        private PddClient _client;
        public OpenController(ILogger<OpenController> logger, PddDbContext dbContext, SiteConfig siteConfig, PddClient client) : base(logger, dbContext, siteConfig)
        {
            _client = client;
        }
        [HttpGet, AllowAnonymous]
        public IActionResult QueryOrder(string orderSn)
        {
            Assert.IsNullOrEmpty(orderSn, "订单号");
            var order = DbContext.Order.FirstOrDefault(x => x.OrderSn.Equals(orderSn));
            Assert.IsNull(order, "订单不存在");
            return Success(new
            {
                order.OrderSn,
                order.OrderStatus,
                PayTime=order.ConfirmTime,
                Amount = int.Parse((order.OrderMoney * 100).ToString("0"))
            });
        }
        [HttpGet, AllowAnonymous]
        public IActionResult QueryGoods(long mallId)
        {
            var mallInfo = DbContext.Mall.FirstOrDefault(x => x.OwnerId == mallId.ToString());
            Assert.IsNull(mallInfo, "店铺不存在");
            var response = _client.Request(new GetGoodsListRequestModel()
            {
                IsOnsale = 1
            }, mallInfo.AccessToken);
            var list = new List<object>();
            foreach (var goodsItem in response.GoodsListGetResponse?.GoodsList)
            {
                foreach (var skuItem in goodsItem.SkuList)
                {
                    if (1.Equals(skuItem.IsSkuOnsale))
                    {
                        list.Add(new
                        {
                            goodsItem.GoodsId,
                            goodsItem.GoodsName,
                            skuItem.SkuId,
                        });
                    }
                }
            }
            return SuccessList(list);

        }
    }
}