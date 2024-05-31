using Aliyun.OSS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddService.Common;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using Serialize.Linq.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace PddService.Code.AsyncLogicHandler.Handlers
{
    public class ExportOrderHandler : AbsAsyncLoginHandlerBase<AsyncTask>
    {
        IOss ossClient;
        private string bucketName;
        private string endpoint;
        public ExportOrderHandler(ILogger<ExportOrderHandler> logger, PddDbContext dbContext, PddClient pddClient, ITokenContainer tokenContainer, IOss ossClient, IConfiguration configuration) : base(logger, dbContext, pddClient, tokenContainer)
        {
            this.ossClient = ossClient;
            this.bucketName = configuration.GetSection("Aliyun:Oss:BucketName").Value;
            this.endpoint = configuration.GetSection("Aliyun:Oss:Endpoint").Value;
        }

        public override string MessageTypeName => AsyncHandlerConstant.ExportOrder.ToString();
        private int pageSize = 50;
        public override Task Execute(AsyncTask request)
        {
            try {
                var serializer = new ExpressionSerializer(new JsonSerializer());
                var expression = (Expression<Func<Order, bool>>)serializer.DeserializeText(request.TaskParam);
                var query = DbContext.Order
                    .Include(x => x.Express)
                    .Include(x => x.Mall)
                    .Include(x => x.OrderDetail)
                    .AsNoTracking()
                    .Where(expression.Compile());
                var exportExcel = ExportExcel<Order>.Create();
                BuildCell(exportExcel);
                var total = query.Count();

                var index = 0;
                while (index < total)
                {
                    var list = query.Skip(index).Take(pageSize);
                    exportExcel.SetDataSet(list);
                    index += pageSize;
                }
                var resUrl = UpdateToOss(request, exportExcel.ToBytes());
                DbContext.AsyncTask.Where(x => x.Id == request.Id).Update(x => new AsyncTask
                {
                    CompleteTime = DateTime.Now,
                    CompleteParam = resUrl,
                    TaskStatus = 1
                });
                DbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                Logger.LogDebug(ex, "导出Excel过程中出现异常");
                DbContext.AsyncTask.Where(x => x.Id == request.Id).Update(x => new AsyncTask
                {
                    CompleteTime = DateTime.Now,
                    TaskStatus = 2
                });
                DbContext.SaveChanges();
            }

            return Task.CompletedTask;
        }

        private string UpdateToOss(AsyncTask asyncTask, byte[] buffer)
        {
            var objectKey = $"export/{asyncTask.TaskKey}_{asyncTask.Id}.xls";
            var putResponse = ossClient.PutObject(bucketName, objectKey, new MemoryStream(buffer));
            var resUrl = $"https://{bucketName}.{endpoint}/{objectKey}";
            return resUrl;
        }


        private void BuildCell(ExportExcel<Order> exportExcel)
        {
            var rowIndex = 1;
            exportExcel["序号"] = x => rowIndex++;
            exportExcel["多多单号"] = x => x.OrderSn;
            exportExcel["店铺"] = x => x.Mall.MallName;
            exportExcel["成交时间"] = x => x.ConfirmTime.ToString("yyyy-MM-dd HH:mm:ss");
            exportExcel["收件人"] = x => x.ReceiverName;
            exportExcel["手机"] = x => x.ReceiverPhone;
            exportExcel["省"] = x => x.Province;
            exportExcel["市"] = x => x.City;
            exportExcel["区"] = x => x.Town;
            exportExcel["地址"] = x => x.Address;
            exportExcel["金额"] = x => x.OrderMoney.ToString("C");
            exportExcel["数量"] = x => x.OrderNum;
            exportExcel["发货状态"] = x => x.WaybillStatus ? "已发货" : "未发货";
            exportExcel["快递公司"] = x => x.Express?.Name ?? "-";
            exportExcel["快递单号"] = x => x.TrackingNumber ?? "-";
            exportExcel["打印状态"] = x => x.PrintStatus ? "已打印" : "未打印";
            exportExcel["订单状态"] = x => OrderStatusDescribe.GetOrderStatusDescribe(x.OrderStatus);
            exportExcel["退款状态"] = x => OrderStatusDescribe.GetOrderAfterSaleStatusDescribe(x.AfterSalesStatus);
        }
    }
}
