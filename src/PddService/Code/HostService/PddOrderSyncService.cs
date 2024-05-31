using CSRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PddOpenSDK.Subscribe;
using PddOpenSDK.Subscribe.Order;
using PddService.Code.PddSubscribeHandle;
using PddService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PddService.Code.HostService
{
    public class PddOrderSyncService : IHostedService
    {
        private bool _isRun = true;
        private ILogger<PddOrderSyncService> logger { get; }
        private Thread thread;
        private IServiceScope serviceScope;
        private CSRedisClient redisClient;
        private string queueName;

        private IDictionary<string, ISubscribeHandle> handleDict = new Dictionary<string, ISubscribeHandle>();
        private readonly IRequestTrace requestTrace;
        private readonly Semaphore semaphore = new Semaphore(5, 5);

        public PddOrderSyncService(ILogger<PddOrderSyncService> logger, IServiceProvider serviceProvider,
            CSRedisClient redisClient, IConfiguration configuration, IRequestTrace requestTrace)
        {
            this.logger = logger;
            //this.serviceProvider = serviceProvider;
            this.FindHandleImpl(serviceProvider);

            this.redisClient = redisClient;
            this.requestTrace = requestTrace;
            queueName = configuration.GetSection("SyncOrderQueue").Value ?? "SyncOrderQueue";
        }

        private void FindHandleImpl(IServiceProvider serviceProvider)
        {
            serviceScope = serviceProvider.CreateScope();
            var services = serviceScope.ServiceProvider.GetServices<ISubscribeHandle>();
            foreach (var impl in services)
            {
                handleDict[impl.MessageTypeName] = impl;
            }

            thread = new Thread(ReadMessage);
        }

        private void ReadMessage()
        {
            logger.LogInformation("订单同步任务开始执行");
            byte number = 0;
            while (_isRun)
            {
                try
                {
                    var messageBody = redisClient.LPop(queueName);
                    if (string.IsNullOrEmpty(messageBody))
                    {
                        Thread.Sleep(300);
                        if (number > byte.MaxValue)
                        {
                            logger.LogInformation("订单同步任务正在执行中");
                            number = 0;
                        }
                        number++;
                        continue;
                    }
                    requestTrace.Trace();
                    logger.LogInformation($"获取到队列消息Content={messageBody}");
                    var pushMessage = JsonConvert.DeserializeObject<PushMessage>(messageBody);
                    handleDict.TryGetValue(pushMessage.Type, out var handle);
                    if (handle != null)
                    {
                        semaphore.WaitOne();
                        Task.Run(() =>
                        {
                            var success = handle.MessageHandle(pushMessage.Content);
                            logger.LogInformation($"消息处理结果：{(success ? "成功" : "失败")}");
                        }).ContinueWith(task => {
                            semaphore.Release();
                        });
                       
                    }
                    else
                    {
                        logger.LogInformation($"消息没有匹配到合适的处理器，跳过");
                    }
                    requestTrace.Clear();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "同步订单服务出错");
                    Thread.Sleep(300);
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _isRun = true;
            thread.Start();
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _isRun = false;
            thread.Abort();
            return Task.CompletedTask;
        }
    }
}