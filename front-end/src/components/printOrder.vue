<template>
<div class="drawer_content" ref="content">
    <el-form ref="form">
        <el-form-item label="打印机列表" label-width="100px">
            <el-select v-model="printName" placeholder="打印机">
                <el-option v-for="item in printList" :key="item.name" :label="item.name" :value="item.name">
                </el-option>
            </el-select>
            <el-button icon="el-icon-refresh-right" size="small" circle v-on:click="loadPrints"></el-button>
        </el-form-item>
        <el-table :height="tableHeight" v-if="tableHeight" :data="gridData">
            <el-table-column property="orderSn" label="单号" width="120"></el-table-column>
            <el-table-column property="express.name" label="快递名" width="120"></el-table-column>
            <el-table-column property="trackingNumber" label="单号"></el-table-column>
            <el-table-column :formatter="formatprintStatus" label="打印状态" width="80"></el-table-column>
        </el-table>
    </el-form>
    <div class="drawer_footer">
        <el-button type="primary" @click="onPrintExpress" :loading="loading">{{ loading ? '打印中 ...' : `打印快递单` }}</el-button>
        <!-- <el-button type="primary" @click="onPrintOrder" :loading="loading">{{ loading ? '打印中 ...' : `打印发货单` }}</el-button> -->
        <el-button @click="onCancel">关 闭</el-button>
    </div>
</div>
</template>

<style scoped>
.drawer_content {
    margin        : 0px 10px;
    display       : flex;
    flex-direction: column;
    height        : 95%;
}

.drawer_content form {
    display   : block;
    flex      : 1;
    margin-top: 0em;
}

.drawer_footer {
    display: flex;
}

.drawer_footer button {
    flex: 1;
}
</style>

<script>
import util from "../common/js/util";

export default {
    props: {
        gridData: {
            type: Array,
        },
    },
    data() {
        return {
            loading    : false,
            printList  : [],
            printName  : '',
            tableHeight: 0,
            print      : this.$store.state.$print
        };
    },

    methods: {
        formatprintStatus(val) {
            return val.printStatus ? "已打印": "-";
        },
        onCancel() {
            this.$emit("onCancel");
        },
        async startPrintExpress() {
            let $self         = this;
                $self.loading = true;
            var list          = [];
            for (let index in $self.gridData) {
                let model              = $self.gridData[index];
                var obj                = new Object();
                    obj.orderSn        = model.orderSn;
                    obj.trackingNumber = model.trackingNumber;
                    obj.printData      = eval("(" + model.printData + ")");
                list.push(obj);
            }
            try {
                let response = await $self.print.printDocument($self.printName, list, 5000);
                if (response.status == "success") {
                    $self.$alert('打印任务已提交，请等待打印机打印', '提示');
                }
                $self.loading = false;
            } catch (e) {
                $self.loading = false;
                $self.msgError("菜鸟组件打印失败：" + e.message);
            }
        },
        async onPrintExpress() {
            let $self = this;
            let count = $self.$linq.from($self.gridData).count(x => x.printStatus);
            if (count == 0) {
                $self.startPrintExpress();
                return;
            }
            this.$confirm(`该批订单有${count}条已打印, 是否继续打印?`, '提示', {
                cancelButtonText : '取消',
                confirmButtonText: '确定',
                type             : 'warning'
            }).then(() => {
                $self.startPrintExpress();
            }).catch(() => {});
        },
        async onPrintOrder() {

        },
        async loadPrints() {
            let $self = this;
            try {
                let response        = await $self.print.getPrinters(500);
                    $self.printList = response.printers;
                    $self.printName = response.defaultPrinter;
            } catch (e) {
                $self.msgError("菜鸟组件获取打印机失败：" + e.message);
            }
        }
    },
    mounted: function () {
        this.tableHeight = (this.$refs.content.offsetHeight - 120) + 'px';
        this.loadPrints();
    }
};
</script>
