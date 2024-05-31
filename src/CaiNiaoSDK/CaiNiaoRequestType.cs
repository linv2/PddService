using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CaiNiaoSDK.Attribute;
using CaiNiaoSDK.Models;

namespace PddOpenSDK
{
    public static class CaiNiaoRequestType
    {
        private static ConcurrentDictionary<string, string> TypeDictionary { get; }
        static CaiNiaoRequestType()
        {
            TypeDictionary = new ConcurrentDictionary<string, string>();
        }
        public static string GetRequestTypeName<TResponse>(this CaiNiaoRequestModel<TResponse> model) where TResponse: CaiNiaoResponseModel
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
                var requestMethodAttribute = type.GetCustomAttributes<CaiNiaoRequestMethodAttribute>().FirstOrDefault();
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
