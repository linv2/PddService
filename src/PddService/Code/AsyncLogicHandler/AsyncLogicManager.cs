using CSRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PddService.Code.AsyncLogicHandler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.AsyncLogicHandler
{
    public class AsyncLogicManager
    {
        ILogger<AsyncLogicManager> logger;
        CSRedisClient redisClient;
        string queueName;
        public AsyncLogicManager(ILogger<AsyncLogicManager> logger, CSRedisClient redisClient, IConfiguration configuration)
        {
            this.logger = logger;
            this.redisClient = redisClient;
            queueName = configuration.GetSection("SyncLogicQueue").Value ?? "SyncLogicQueue";
        }
        public void Push<TParam>(AsyncHandlerConstant command, TParam param)
        {
            try
            {
                var messageBody = JsonConvert.SerializeObject(param);
                logger.LogDebug($"异步业务消息准备写入队列 Command={command.ToString()} Content={messageBody}");
                var model = new AsyncLogicModel()
                {
                    MessageType = command.ToString(),
                    Content = messageBody
                };
                redisClient.RPush(queueName, model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "异步业务消息写入队列失败");
            }
        }

        public async Task PushAsync<TParam>(AsyncHandlerConstant command, TParam param)
        {
            try
            {
                var messageBody = JsonConvert.SerializeObject(param);
                logger.LogDebug($"异步业务消息准备写入队列 Command={command.ToString()} Content={messageBody}");
                var model = new AsyncLogicModel()
                {
                    MessageType = command.ToString(),
                    Content = messageBody
                };
                var resValu = await redisClient.RPushAsync(queueName, model);
                logger.LogDebug($"异步业务消息准备写入写入Id:{resValu}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "异步业务消息写入队列失败");
            }
        }
        public AsyncLogicModel Pop()
        {
            try
            {
                return redisClient.LPop<AsyncLogicModel>(queueName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "异步业务消息读取失败");
                return null;
            }
        }
    }
}
