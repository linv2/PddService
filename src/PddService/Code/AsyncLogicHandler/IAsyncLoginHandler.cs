using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.AsyncLogicHandler
{
    public interface IAsyncLoginHandler
    {
        string MessageTypeName { get; }
        void MessageHandle(string messageBody);
        Task MessageHandleAsync(string messageBody);
    }
}
