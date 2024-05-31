using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PddOpenSDK.Attribute;
using PddOpenSDK.Models;

namespace PddOpenSDK
{
    public static class PddRequestType
    {
        private static ConcurrentDictionary<string, string> TypeDictionary { get; }
        static PddRequestType()
        {
            TypeDictionary = new ConcurrentDictionary<string, string>();
        }
        public static string GetRequestTypeName<TResponse>(this PddRequestModel<TResponse> model) where TResponse:PddResponseModel
        {
            if (model == null)
            {
                throw new NullReferenceException();
            }

            var modelType = model.GetType();
            var requestTypeName = GetTypeName(modelType);
            return requestTypeName;
        }

        private static string GetTypeName(Type type)
        {
            string resultValue;
            var typeName = type.FullName;
            if (!TypeDictionary.TryGetValue(typeName, out resultValue))
            {
                var requestMethodAttribute = type.GetCustomAttributes<PddRequestMethodAttribute>().FirstOrDefault();
                if (requestMethodAttribute == null || string.IsNullOrEmpty(requestMethodAttribute.TypeName))
                {
                    throw new System.Exception($"{typeName}不存在特性PddRequestMethodAttribute");

                }

                resultValue = requestMethodAttribute.TypeName;
                TypeDictionary.TryAdd(typeName, resultValue);
            }

            return resultValue;
        }
    }
}
