using PddService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code
{
    public class RequestTraceService : IRequestTrace
    {
        public void Clear(string propertyName = "reqId")
        {
            log4net.ThreadContext.Properties.Remove(propertyName);
        }

        public void Trace(string propertyName = "reqId")
        {
            log4net.ThreadContext.Properties[propertyName] = Snowflake.Instance.nextId();
        }
    }
}
