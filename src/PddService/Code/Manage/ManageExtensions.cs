using Microsoft.Extensions.DependencyInjection;
using PddService.Code.AsyncLogicHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PddService.Code.Manage
{
    public static class ManageExtensions
    {
        const string StartName = "PddService.Code";
        const string EndName = "Manage";
        public static IServiceCollection AddPddServiceManage(this IServiceCollection servicese)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                  .Where(x => x.IsClass && x.FullName.StartsWith(StartName)&&x.FullName.EndsWith(EndName));
            foreach (var type in types)
            {
                servicese.AddScoped(type);
            }
            servicese.AddSingleton<AsyncLogicManager>();
            return servicese;
        }
    }
}
