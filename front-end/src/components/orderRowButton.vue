<template>
<el-button-group>
    <el-button size="mini" @click="doWaybill" v-if="checkButton('waybill')">发货</el-button>
    <el-button size="mini" @click="doPrint" v-if="checkButton('print')">打印</el-button>
    <el-button size="mini" @click="doSyncOrderStatus" v-if="checkButton('sync')">更新状态</el-button>
</el-button-group>
</template>

<script>
export default {
    props  : ["order", "index", "showBtns"],
    methods: {
        checkButton(typeName) {
            return this.showBtns.indexOf(typeName) > -1;
        },
        async doPrint() {
            this.$emit("onPrint", this.index, this.order);
        },
        async doSyncOrderStatus() {
            var $self = this;
            let resp  = await $self.doPostSync("/order/syncorderstatus", [$self.order.id]);
            $self.msgShow(resp)
            resp.code == 200 && $self.updateOrder();
        },
        async updateOrder() {
            var $self = this;
            let resp  = await $self.doGetSync("/order/get", {
                id: $self.order.id
            });
            this.$emit("update", $self.index, resp.data);
        },
        async doWaybill() {
            var $self = this;
            $self.doGet("/order/SingleWayBill", {
                id: $self.order.id
            }, {
                success(res) {
                    return true;
                },
                fail(res) {}
            });
        }
    }
}
</script>
