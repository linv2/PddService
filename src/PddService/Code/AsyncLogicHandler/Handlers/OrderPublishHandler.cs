using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PddOpenSDK;
using PddService.Code.AsyncLogicHandler.Model;
using PddService.Common;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PddService.Code.AsyncLogicHandler.Handlers
{
    public class OrderPublishHandler : AbsAsyncLoginHandlerBase<OrderPublishModel>
    {
        AsyncLogicManager asyncLogicManager;
        public OrderPublishHandler(ILogger<OrderPublishHandler> logger, PddDbContext dbContext, PddClient pddClient, ITokenContainer tokenContainer, AsyncLogicManager asyncLogicManager) : base(logger, dbContext, pddClient, tokenContainer)
        {
            this.asyncLogicManager = asyncLogicManager;
        }

        public override string MessageTypeName => AsyncHandlerConstant.OrderPublish.ToString();

        public override Task Execute(OrderPublishModel request)
        {
            var order = DbContext
                  .Order
                  .AsNoTracking()
                  .Include(x => x.User)
                  .FirstOrDefault(x => x.Id == request.OrderId);
            if (!TryPublish(order).Result)
            {
                if (request.Number > 3)
                {
                    return Task.CompletedTask;
                }
                Logger.LogDebug($"订单OrderId={order.Id}第{request.Number}次通知失败");
                request.Number++;
                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(request.Number * 10 * 1000);
                    asyncLogicManager.Push(AsyncHandlerConstant.OrderPublish, request);
                });

            }
            else
            {
                Logger.LogDebug($"订单OrderId={order.Id}通知成功");
            }
            return Task.CompletedTask;
        }
        private async Task<bool> TryPublish(Order order)
        {
            try
            {
                var httpClent = new HttpClient();
                var requestBody = BuildRequestBody(order);
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(order.User.NotityUrl);
                request.Content = new StringContent(requestBody);
                Logger.LogDebug("Notity Request Content=" + requestBody);
                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await httpClent.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Logger.LogDebug("Notity Response Content=" + responseContent);
                    return "OK".Equals(responseContent);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"订单Order={order.Id}Http Publish异常");
            }
            return false;

        }
        private string BuildRequestBody(Order order)
        {
            var requestBody = new
            {
                order.OrderSn,
                order.OrderStatus,
                MallId = order.OwnerId,
                Mobile = order.ReceiverPhone,
                PayTime=order.ConfirmTime.ToString("yyyy-MM-dd HH:mm:ss")
            };
            var requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var strRequestBody = JsonConvert.SerializeObject(requestBody);
            var reqSign = (strRequestBody + requestTime + order.User.NotityKey).Md5();
            var request = new
            {
                requestTime,
                requestBody = strRequestBody,
                reqSign
            };
            return JsonConvert.SerializeObject(request);


        }
    }
}
