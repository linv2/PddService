using Aliyun.OSS;
using CaiNiaoSDK;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PddOpenSDK;
using PddService.Code;
using PddService.Code.HostService;
using PddService.Code.Manage;
using PddService.Code.PddSubscribeHandle;
using PddService.Common;
using PddService.Common.Filters;
using PddService.Common.JsonConverter;
using PddService.Common.ModelBinder.Json.Net;
using PddService.DataAccess;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PddService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        private ILoggerFactory LoggerFactory { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Add Mvc
            services.AddControllers(option =>
            {
                option.Filters.Add<ExceptionFilter>();
                option.Filters.Add<AuthorizeFilter>();
                option.Filters.Add<RequestLoggerFilter>();
                option.ModelBinderProviders.Insert(0, new JObjectModelBinderProvider());
            })
               .ConfigureApiBehaviorOptions(options =>
               {
                   options.SuppressModelStateInvalidFilter = true;
                   options.SuppressConsumesConstraintForFormFileParameters = true;
               })
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                   options.JsonSerializerOptions.Converters.Add(new DateTimeNullConverter());
               })
               .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            #endregion

            #region Add DbConnection


            services.AddDbContext<PddDbContext>(options =>
            {
                options.UseMySql(Configuration.GetSection("ConnectionStings:DbConnection")?.Value ??
                                 throw new InvalidOperationException("数据库配置无效"),ServerVersion.AutoDetect(Configuration.GetSection("ConnectionStings:DbConnection")?.Value));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            #endregion

            services.AddSession(options => { options.Cookie.Name = "sessionId"; });


            services.AddMemoryCache();
            var redisClient = new CSRedis.CSRedisClient(
                Configuration.GetSection("ConnectionStings:RedisConfig")?.Value ??
                throw new InvalidOperationException("Redis配置无效"));
            services.AddSingleton(redisClient);
            services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(redisClient));
            services.AddSingleton<PddClient>(provider =>
            {
                var clientId = Configuration.GetSection("PddConfig:ClientId").Value ?? "";
                var clientSecret = Configuration.GetSection("PddConfig:ClientSecret").Value ?? "";
                return new DefaultPddClient(clientId, clientSecret);
            });
            services.AddSingleton<CaiNiaoClient>(provider =>
            {
                var appKey = Configuration.GetSection("CaiNiao:AppId").Value ?? "";
                var appSecret = Configuration.GetSection("CaiNiao:AppSecret").Value ?? "";
                return new DefaultCaiNiaoClient(appKey, appSecret);
            });
            services.AddSingleton(provider => new SiteConfig
            {
                Url = Configuration.GetSection("SiteConfig:Url").Value ?? ""
            });


            services
                .AddPddServiceManage()
                .AddPddSubscribeManage()
                .AddAsyncLogicManage()
                .AddHostedService<AsyncLogicService>().AddHostedService<PddOrderSyncService>();
            services.AddScoped<ITokenContainer, TokenContainer>();


            #region Swagger

            services.AddScoped<SwaggerGenerator>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", Configuration.GetValue<OpenApiInfo>("Swagger", new OpenApiInfo
                {
                    Title = "API列表",
                    Version = "v1",
                    Description = "打单集成小助手API列表",
                    Contact = new OpenApiContact()
                    {
                        Name = "Linv2",
                        Email = "",
                        Url = new Uri(Configuration.GetSection("SiteConfig:Url").Value)
                    }
                }));
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PddService.DataAccess.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CaiNiaoSDK.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PddOpenSDK.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PddService.Common.xml"));
            });

            #endregion


            #region Add OssClient
            services.AddSingleton(x =>
            {
                var accessId = Configuration.GetSection("Aliyun:AccessId").Value ?? "";
                var accessKey = Configuration.GetSection("Aliyun:AccessKey").Value ?? "";
                var endpoint = Configuration.GetSection("Aliyun:Oss:Endpoint").Value ?? "";
                IOss ossClient = new OssClient(endpoint, accessId, accessKey);
                return ossClient;
            });
            #endregion

            services.AddSingleton<IRequestTrace, RequestTraceService>();

            services.AddHttpClient();
            services.AddHangfire(configuration =>
            {

            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PddDbContext context, ILoggerFactory loggerFactory)
        {
            #region Add Logging Service
            loggerFactory.AddLog4Net(Path.Combine(env.ContentRootPath, "log4net.config"));
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/swagger.json"; });
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "api-docs";
                options.SwaggerEndpoint("/api-docs/v1/swagger.json", "api document"); //css注入
            });


            var corsOrigins = Configuration.GetSection("Cros:Origins").Get<string[]>();
            if (corsOrigins?.Any() ?? false)
            {
                app.UseCors(builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins(corsOrigins);
                });
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization().UseSession();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            DbInitializer.Initializer(context);
        }
    }
}