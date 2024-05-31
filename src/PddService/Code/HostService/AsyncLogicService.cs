using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PddService.Code.AsyncLogicHandler;
using PddService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PddService.Code.HostService
{
    public class AsyncLogicService : IHostedService
    {
        private bool _isRun = true;
        private ILogger<AsyncLogicService> logger { get; }
        private Thread thread;
        private IServiceScope serviceScope;
        AsyncLogicManager syncLogicManager;
        private IDictionary<string, IAsyncLoginHandler> handleDict = new Dictionary<string, IAsyncLoginHandler>();
        private readonly IRequestTrace requestTrace;
        private readonly Semaphore semaphore = new Semaphore(5, 5);

        public AsyncLogicService(ILogger<AsyncLogicService> logger, IServiceProvider serviceProvider, AsyncLogicManager syncLogicManager, IRequestTrace requestTrace)
        {
            this.logger = logger;
            this.syncLogicManager = syncLogicManager;
            this.requestTrace = requestTrace;
            //this.serviceProvider = serviceProvider;
            this.FindHandleImpl(serviceProvider);

        }

        private void FindHandleImpl(IServiceProvider serviceProvider)
        {
            serviceScope = serviceProvider.CreateScope();
            var services = serviceScope.ServiceProvider.GetServices<IAsyncLoginHandler>();
            foreach (var impl in services)
            {
                handleDict[impl.MessageTypeName] = impl;
            }

            thread = new Thread(ReadMessage);
        }

        private void ReadMessage()
        {
            logger.LogInformation("异步业务任务开始执行");
            byte number = 0;
            while (_isRun)
            {
                try
                {

                    var syncLogicModel = syncLogicManager.Pop();
                    if (syncLogicModel != null)
                    {
                        logger.LogInformation($"获取到异步业务消息Content={syncLogicModel.Content}");
                        handleDict.TryGetValue(syncLogicModel.MessageType, out var handle);
                        if (handle != null)
                        {
                            semaphore.WaitOne();
                            handle.MessageHandleAsync(syncLogicModel.Content).ContinueWith(task =>
                            {
                                semaphore.Release();
                            });
                        }
                        else
                        {
                            logger.LogInformation($"异步业务消息没有匹配到合适的处理器 Command={syncLogicModel.MessageType}");
                        }
                    }
                    else
                    {
                        if (number > byte.MaxValue)
                        {
                            logger.LogInformation("异步业务任务正在执行中");
                            number = 0;
                        }
                        number++;
                        Thread.Sleep(500);
                    }

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
