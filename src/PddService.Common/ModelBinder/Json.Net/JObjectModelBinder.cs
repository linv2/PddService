using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace PddService.Common.ModelBinder.Json.Net
{
    public class JObjectModelBinder : IModelBinder
    {
        public JObjectModelBinder(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
        }
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException("bindingContext");
            var context = bindingContext.ActionContext.HttpContext;
            try
            {
                if (bindingContext.ModelType == typeof(JObject))
                {
                    if (bindingContext.BindingSource == BindingSource.Body)
                    {
                        var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
                        await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                        var json = Encoding.UTF8.GetString(buffer);
                        bindingContext.Result = ModelBindingResult.Success(JObject.Parse(json));
                    }
                    else if (bindingContext.BindingSource == BindingSource.Form)
                    {
                        JObject jObject = new JObject();
                        foreach (var item in bindingContext.ActionContext.HttpContext.Request.Form)
                        {
                            jObject.Add(new JProperty(item.Key, item.Value));
                        }
                        bindingContext.Result = (ModelBindingResult.Success(jObject));
                    }
                    else if (bindingContext.BindingSource == BindingSource.Query)
                    {
                        JObject jObject = new JObject();
                        foreach (var item in bindingContext.ActionContext.HttpContext.Request.Query)
                        {
                            jObject.Add(new JProperty(item.Key, item.Value));
                        }
                        bindingContext.Result = (ModelBindingResult.Success(jObject));
                    }
                    else
                    {
                        bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "不支持的参数特性");
                    }
                }
            }
            catch (Exception exception)
            {
                if (!(exception is FormatException) && (exception.InnerException != null))
                {
                    exception = ExceptionDispatchInfo.Capture(exception.InnerException).SourceException;
                }
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, exception, bindingContext.ModelMetadata);
            }
        }
    }
}