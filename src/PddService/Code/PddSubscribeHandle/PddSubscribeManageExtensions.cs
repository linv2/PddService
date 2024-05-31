using Microsoft.Extensions.DependencyInjection;
using PddService.Code.AsyncLogicHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PddService.Code.PddSubscribeHandle
{
    public static class ManageExtensions
    {
        public static IServiceCollection AddPddSubscribeManage(this IServiceCollection servicese)
        {
            var subscribeHandleType = typeof(ISubscribeHandle);
            var subscribeHandleFullName = subscribeHandleType.FullName;
            var types = Assembly.GetExecutingAssembly().GetTypes()
                  .Where(x => x.IsClass &&!x.IsAbstract&& x.GetInterfaces().Any(x => subscribeHandleFullName.Equals(x.FullName)));
            foreach(var type in types)
            {
                servicese.AddScoped(subscribeHandleType, type);
            }
            return servicese;
        }

        public static IServiceCollection AddAsyncLogicManage(this IServiceCollection servicese)
        {
            var asyncLoginHandlerType = typeof(IAsyncLoginHandler);
            var asyncLoginHandlerFullName = asyncLoginHandlerType.FullName;
            var types = Assembly.GetExecutingAssembly().GetTypes()
                  .Where(x => x.IsClass && !x.IsAbstract && x.GetInterfaces().Any(x => asyncLoginHandlerFullName.Equals(x.FullName)));
            foreach (var type in types)
            {
                servicese.AddScoped(asyncLoginHandlerType, type);
            }
            return servicese;
        }
    }
}
