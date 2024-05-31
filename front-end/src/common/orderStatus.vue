<script>
export default {
    afterSaleStatus: {
        "0" : "无售后",
        "2" : "买家申请退款，待商家处理",
        "3" : "退货退款，待商家处理",
        "4" : "商家同意退款，退款中",
        "5" : "平台同意退款，退款中",
        "6" : "驳回退款，待买家处理",
        "7" : "已同意退货退款,待用户发货",
        "8" : "平台处理中",
        "9" : "平台拒绝退款，退款关闭",
        "10": "退款成功",
        "11": "买家撤销",
        "12": "买家逾期未处理，退款失败",
        "13": "买家逾期，超过有效期",
        "14": "换货补寄待商家处理",
        "15": "换货补寄待用户处理",
        "16": "换货补寄成功",
        "17": "换货补寄失败",
        "18": "换货补寄待用户确认完成"
    },
    orderStatus: {
        "1": "待发货",
        "2": "已发货待签收",
        "3": "已签收"
    },
    refundStatus: {
        "1": "无售后或售后关闭",
        "2": "售后处理中",
        "3": "退款中",
        "4": "退款成功"
    },
    formatAfterSalesStatus(val) {
        switch (val) {
            case 0: 
                return "无售后";
            case 2 : 
            case 3 : 
            case 14: 
                return "待商家处理";
            case 4: 
            case 5: 
                return "退款中";
            case 6 : 
            case 7 : 
            case 15: 
            case 18: 
                return "待买家处理";
            case 8: 
                return "平台处理中";
            case 9: 
                return "退款关闭";
            case 10: 
                return "退款成功";
            case 11: 
                return "买家撤销";
            case 12: 
                return "退款失败";
            case 13: 
                return "超过有效期";
            case 16: 
                return "补寄成功";
            case 17: 
                return "补寄失败";

        }
    }

}
</script>
