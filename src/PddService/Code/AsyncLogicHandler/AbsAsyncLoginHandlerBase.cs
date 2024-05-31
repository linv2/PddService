using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PddOpenSDK;
using PddService.Code.AsyncLogicHandler.Model;
using PddService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.AsyncLogicHandler
{
    public abstract class AbsAsyncLoginHandlerBase<TRequest> : IAsyncLoginHandler
    {
        protected PddClient PddClient { get; }
        protected ILogger<AbsAsyncLoginHandlerBase<TRequest>> Logger { get; }
        protected PddDbContext DbContext { get; }
        protected ITokenContainer TokenContainer { get; set; }
        protected AbsAsyncLoginHandlerBase(ILogger<AbsAsyncLoginHandlerBase<TRequest>> logger, PddDbContext dbContext, PddClient pddClient, ITokenContainer tokenContainer)
        {
            Logger = logger;
            DbContext = dbContext;
            PddClient = pddClient;
            TokenContainer = tokenContainer;
        }
        public abstract string MessageTypeName { get; }
        public abstract Task Execute(TRequest request);
        public async void MessageHandle(string messageBody)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<TRequest>(messageBody);
                await Execute(request);
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"执行异步任务异常,消息内容={messageBody}");
            }
        }

        public async Task MessageHandleAsync(string messageBody)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<TRequest>(messageBody);
                await Execute(request);
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"执行异步任务异常,消息内容={messageBody}");
            }
        }
    }
}
