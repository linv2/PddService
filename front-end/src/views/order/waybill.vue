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
                <el-button type="primary" v-on:click="doSearch">查询</el-button>
            </el-form-item>
        </el-form>
    </el-col>
    <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
        <el-form :inline="true" :model="filters">
            <el-form-item>
                <el-button type="primary" size="small" v-on:click="doBatchWaybill">发货</el-button>
            </el-form-item>
        </el-form>
    </el-col>
    <el-table :data="list" highlight-current-row v-loading="listLoading" style="width: 100%;" @selection-change="handleSelectionChange">
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
        <el-table-column prop="confirmTime" label="成交时间" width="150"></el-table-column>
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
        <el-table-column prop="orderNum" label="订单数" width="80"></el-table-column>
        <el-table-column prop="orderMoney" :formatter="formatMoney" label="金额" width="80"></el-table-column>
        <el-table-column :formatter="formatOrderStatus" label="订单状态" width="100"></el-table-column>
        <el-table-column label="备注" width="140">
            <template slot-scope="scope">
                <template v-if="scope.row.remark">
                    {{scope.row.remark}}
                </template>
                <template v-else>
                    -
                </template>
            </template>
        </el-table-column>
        <el-table-column label="卖家留言" width="140">
            <template slot-scope="scope">
                <template v-if="scope.row.buyerMemo">
                    {{scope.row.buyerMemo}}
                </template>
                <template v-else>
                    -
                </template>
            </template>
        </el-table-column>
        <el-table-column label="操作">
            <template slot-scope="scope">
                <orderRowButton :order="scope.row" :index="scope.index" showBtns="['waybill','sync']" @update="onUpdate"></orderRowButton>
            </template>
        </el-table-column>
    </el-table>

    <!--工具条-->
    <el-col :span="24" class="toolbar">
        <el-pagination layout="total, sizes, prev, pager, next, jumper" :page-sizes="[50,100, 200, 300, 400]" @current-change="handleCurrentChange" @size-change="handleSizeChange" :page-size="50" :total="total" style="float:right;"></el-pagination>
    </el-col>
</section>
</template>

<script>
import util from "../../common/js/util";
import linq from '../../common/js/linq';
import orderRowButton from '../../components/orderRowButton';
import orderStatusDesc from '../../common/orderStatus';
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
                waybillStatus: 0,
                valid        : true,
                startTime    : '',
                endTime      : ''
            },
            list             : [],
            total            : 0,
            listLoading      : false,
            multipleSelection: []
        };
    },
    methods: {
        formatOrderStatus(val) {
            return orderStatusDesc.orderStatus[val.orderStatus] || "-";
        },
        handleSelectionChange(val) {
            this.multipleSelection = val;
        },
        onUpdate() {
            this.doSearch();
        },
        async doBatchWaybill() {
            let $self = this;
            if ($self.multipleSelection.length == 0) {
                $self.msgError("请选择你要发货的订单");
                return;
            }
            let list = linq.from($self.multipleSelection).select(x => x.id).toArray();
            $self.doPost("/order/batchwaybill", list, {
                success(res) {
                    $self.doSearch();
                    return true;
                }
            })
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
                let respData              = await this.doGetSync("/order/list", this.filters);
                    this.list             = respData.data;
                    this.total            = respData.total;
                    this.conditionChanged = false;
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
