<template>
<section>
    <!--工具条-->
    <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
        <el-form :inline="true" :model="filters">
            <el-form-item>
                <el-input v-model="filters.orderSn" placeholder="订单号" clearable class="w120"></el-input>
            </el-form-item>
            <el-form-item>
                <el-input v-model="filters.ownerName" placeholder="店铺名" clearable class="w120"></el-input>
            </el-form-item>
            <el-form-item>
                <el-input v-model="filters.name" placeholder="收货人" clearable class="w120"></el-input>
            </el-form-item>
            <el-form-item>
                <el-input v-model="filters.mobile" placeholder="电话" clearable class="w120"></el-input>
            </el-form-item>
            <el-form-item>
                <el-date-picker v-model="filters.startTime" type="datetime" value-format="yyyy-MM-dd HH:mm:ss" placeholder="开始日期" class="w192">
                </el-date-picker>
            </el-form-item>
            <el-form-item>
                <el-date-picker v-model="filters.endTime" type="datetime" value-format="yyyy-MM-dd HH:mm:ss" placeholder="结束日期" class="w192">
                </el-date-picker>
            </el-form-item>
            <el-form-item>
                <el-select v-model="filters.waybillStatus" placeholder="发货状态" class="w120">
                    <el-option v-for="item in waybillOptions" :key="item.value" :label="item.label" :value="item.value">
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item>
                <el-select v-model="filters.printStatus" placeholder="打印状态" class="w120">
                    <el-option v-for="item in printOptions" :key="item.value" :label="item.label" :value="item.value">
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" v-on:click="doSearch">查询</el-button>
            </el-form-item>
        </el-form>
    </el-col>

    <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
        <el-form :inline="true" :model="filters">
            <el-form-item>
                <el-button type="primary" size="small" v-on:click="onNotity">通知平台</el-button>
                <el-button type="primary" size="small" v-on:click="onExport">导出Excel</el-button>
            </el-form-item>
        </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="list" highlight-current-row v-loading="listLoading" style="width: 100%;" @selection-change="handleSelectionChange" :row-class-name="tableRowClassName">

        <el-table-column type="selection" width="55">
        </el-table-column>
        <el-table-column type="expand">
            <template slot-scope="props">
                <el-table :data="props.row.orderDetail" highlight-current-row>
                    <el-table-column type="index" label="序号" width="50">
                    </el-table-column>
                    <el-table-column prop="goodsName" label="产品" width="140"></el-table-column>
                    <el-table-column label="图片" width="80">
                        <template slot-scope="goodScope">
                            <a :href="goodScope.row.goodsImg" target="_blank">
                                <img :src="goodScope.row.goodsImg" width="50px" height="50px" />
                            </a>
                        </template>
                    </el-table-column>
                    <el-table-column prop="goodsPrice" :formatter="formatMoney" label="单价" width="80"></el-table-column>
                    <el-table-column prop="goodsCount" label="数量" width="80"></el-table-column>
                    <el-table-column label="总价" width="80">
                        <template slot-scope="goodScope">
                            {{formatMoney(goodScope.row.goodsPrice*goodScope.row.goodsCount)}}
                        </template>
                    </el-table-column>
                    <el-table-column prop="skuId" label="规格id" width="200"></el-table-column>
                </el-table>
            </template>
        </el-table-column>
        <el-table-column prop="orderSn" label="多多订单号" width="110"></el-table-column>
        <el-table-column prop="mall.mallName" label="店铺" width="120">
            <template slot-scope="props">
                <a href="javascript:;" @click="searchMall(props.row)">{{props.row.mall.mallName}}</a>
            </template>
        </el-table-column>
        <el-table-column prop="confirmTime" label="成交时间" width="100"></el-table-column>
        <el-table-column label="收件人" width="120">
            <template slot-scope="scope">
                <el-popover trigger="hover" placement="top">
                    <p>姓名: {{ scope.row.receiverName }}</p>
                    <p>手机: {{ scope.row.receiverPhone }}</p>
                    <p>地址: {{ scope.row.address }}</p>
                    <div slot="reference" class="name-wrapper">
                        <el-tag size="medium">{{ scope.row.receiverName }}</el-tag>
                    </div>
                </el-popover>
            </template>
        </el-table-column>
        <el-table-column prop="orderMoney" :formatter="formatMoney" label="金额" width="80"></el-table-column>
        <el-table-column :formatter="formatOrderStatus" label="订单状态" width="100"></el-table-column>
        <el-table-column :formatter="formatRefundStatus" label="退款状态" width="140"></el-table-column>
        <el-table-column label="发货状态" width="60">
            <template slot-scope="scope">
                <el-popover trigger="hover" placement="top" v-if="scope.row.waybillStatus">
                    <p>快 递 : {{ scope.row.express.name }}</p>
                    <p>单 号 : {{ scope.row.trackingNumber }}</p>
                    <p>发货时间: {{ scope.row.shippingTime }}</p>
                    <div slot="reference" class="name-wrapper">
                        <el-tag size="medium" type="success">是</el-tag>
                    </div>
                </el-popover>
                <div v-else>-</div>
            </template>
        </el-table-column>
        <el-table-column :formatter="formatprintStatus" label="打印状态" width="80"></el-table-column>
        <el-table-column :formatter="formatSyncStatus" label="通知状态" width="80"></el-table-column>
        <el-table-column label="售后状态" width="100">
            <template slot-scope="scope">
                <el-popover trigger="hover" placement="top" v-if="scope.row.afterSalesStatus>0">
                    <p>状态：{{ afterSaleStatus[scope.row.afterSalesStatus]}}</p>
                    <div slot="reference" class="name-wrapper">
                        <el-tag size="warning" type="success">{{formatAfterSalesStatus(scope.row.afterSalesStatus)}}</el-tag>
                    </div>
                </el-popover>
                <div v-else>-</div>
            </template>
        </el-table-column>
        <el-table-column label="操作">
            <template slot-scope="scope">
                <orderRowButton :order="scope.row" :index="scope.index" showBtns="['sync']" @update="updateOrder"></orderRowButton>
            </template>
        </el-table-column>
    </el-table>

    <!--工具条-->
    <el-col :span="24" class="toolbar">
        <el-pagination layout="total, sizes, prev, pager, next, jumper" :page-sizes="[50,100, 200, 300, 400]" @current-change="handleCurrentChange" @size-change="handleSizeChange" :page-size="50" :total="total" style="float:right;"></el-pagination>
    </el-col>

</section>
</template>

<style>
a {
    text-decoration: none;
}
</style>

<script>
import axios from "axios";
import util from "../../common/js/util";
import orderStatusDesc from '../../common/orderStatus';
import orderRowButton from '../../components/orderRowButton'
export default {
    components: {
        orderRowButton
    },
    data() {
        return {
            filters: {
                pageIndex    : 1,
                pageSize     : 50,
                ownerName    : "",
                orderSn      : "",
                name         : "",
                mobile       : "",
                waybillStatus: -1,
                printStatus  : -1,
                startTime    : '',
                endTime      : ''
            },
            afterSaleStatus: orderStatusDesc.afterSaleStatus,
            list           : [],
            total          : 0,
            listLoading    : false,
            waybillOptions : [{
                label: "全部",
                value: -1
            }, {
                label: "已发货",
                value: 1
            }, {
                label: "未发货",
                value: 0
            }],
            printOptions: [{
                label: "全部",
                value: -1
            }, {
                label: "已打印",
                value: 1
            }, {
                label: "未打印",
                value: 0
            }],
            multipleSelection: []
        };
    },
    methods: {
        tableRowClassName({
            row,
            rowIndex
        }) {
            return row.afterSalesStatus > 0 && 'warning-row';
        },
        renderWaybilledData(successWaybill) {
            let $self = this;
            this.doPost("/order/updateprintstatus", successWaybill, {
                any() {
                    for (let index in $self.list) {
                        let item = $self.list[index];
                        if (successWaybill.indexOf(item.trackingNumber) > -1) {
                            item.printStatus = true;
                        }
                    }
                }
            });
        },
        updateOrder(index, data) {
            this.list[index] = data;
        },
        handleSelectionChange(val) {
            this.multipleSelection = val;
        },
        formatAfterSalesStatus(val) {
            return orderStatusDesc.formatAfterSalesStatus(val);
        },
        formatprintStatus(val) {
            return val.printStatus ? "已打印": "-";
        },
        formatSyncStatus(val) {
            return val.syncStatus ? "-": "未通知";
        },
        formatOrderStatus(val) {
            return orderStatusDesc.orderStatus[val.orderStatus] || "-";
        },
        formatRefundStatus(val) {

            return orderStatusDesc.refundStatus[val.refundStatus] || "-";
        },
        async onNotity() {
            if (this.multipleSelection.length == 0) {
                this.msgError("没有选中任何要打印的订单");
                return;
            }
            let param = this.$linq.from(this.multipleSelection).select(x => x.id).toArray();
            let resp  = await this.doPostSync("/order/notityplatform", param);
            this.msgShow(resp)
        },
        async onExport() {
            let resp = await this.doGetSync("/order/export", this.filters);
            this.msgShow(resp);
            if (resp.code == 200) {
                this.$store.state.exportExcel.addJob(resp.data);
            }

        },
        handleCurrentChange(val) {
            this.filters.pageIndex = val;
            this.doSearch();
        },
        handleSizeChange(val) {
            this.filters.pageSize = val;
            this.doSearch();
        },
        searchMall(row) {
            this.filters.ownerName = row.mall.mallName;
            this.doSearch();
        },
        //获取用户列表
        async doSearch() {
            this.listLoading = true;
            try {
                let resp       = await this.doGetSync("/order/list", this.filters);
                    this.list  = resp.data;
                    this.total = resp.total;
            } catch (e) {} finally {
                this.listLoading = false;
            }
        }
    },
    mounted() {
        this.doSearch();
    }
};
</script>

<style scoped>
</style>
