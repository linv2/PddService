using System;
using System.Collections.Generic;
using System.Text;

namespace PddService.Common
{
    public class OrderStatusDescribe
    {
        public static string GetOrderStatusDescribe(int orderStatus)
        {
            switch (orderStatus)
            {
                case
                1:
                    return "待发货";
                case
                    2:
                    return "已发货待签收";
                case
                    3:
                    return "已签收";
            }
            return "-";
        }
        public static string GetOrderAfterSaleStatusDescribe(int orderStatus)
        {
            switch (orderStatus)
            {
                case 0: return "无售后";
                case 2: return "买家申请退款，待商家处理";
                case 3: return "退货退款，待商家处理";
                case 4: return "商家同意退款，退款中";
                case 5: return "平台同意退款，退款中";
                case 6: return "驳回退款，待买家处理";
                case 7: return "已同意退货退款,待用户发货";
                case 8: return "平台处理中";
                case 9: return "平台拒绝退款，退款关闭";
                case 10: return "退款成功";
                case 11: return "买家撤销";
                case 12: return "买家逾期未处理，退款失败";
                case 13: return "买家逾期，超过有效期";
                case 14: return "换货补寄待商家处理";
                case 15: return "换货补寄待用户处理";
                case 16: return "换货补寄成功";
                case 17: return "换货补寄失败";
                case 18:
                    return "换货补寄待用户确认完成";
            }
            return "-";
        }
    }
}

