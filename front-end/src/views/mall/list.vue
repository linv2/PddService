<template>
<section>
    <!--工具条-->
    <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
        <el-form :inline="true" :model="filters">
            <el-form-item>
                <el-input v-model="filters.mallName" placeholder="店铺名" clearable></el-input>
            </el-form-item>
            <el-form-item>
                <el-input v-model="filters.ownerId" placeholder="店铺ID" clearable></el-input>
            </el-form-item>
            <el-form-item>
                <el-input v-model="filters.ownerName" placeholder="账号名" clearable></el-input>
            </el-form-item>
            <el-form-item>
                <el-select v-model="filters.valid" placeholder="打印状态" class="w120">
                    <el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value">
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" v-on:click="doSearch">查询</el-button>
            </el-form-item>
            <el-form-item>
                <a :href="oauthUrl" target="_blank" class="el-button el-button--primary">新增店铺</a>
                <el-dropdown split-button type="success" @command="onSyncOrder">同步订单
                    <el-dropdown-menu slot="dropdown">
                        <el-dropdown-item v-for="item in syncTimeArray" :key="item.value" :command="item.value">{{item.label}}</el-dropdown-item>
                    </el-dropdown-menu>
                </el-dropdown>
            </el-form-item>
        </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="users" highlight-current-row v-loading="listLoading" style="width: 100%;" @selection-change="handleSelectionChange">
        <el-table-column type="selection" width="55">
        </el-table-column>
        <el-table-column prop="ownerId" label="多多店铺Id" width="120"></el-table-column>
        <el-table-column prop="mallName" label="店铺名" width="120"></el-table-column>
        <el-table-column prop="merchantType" label="店铺类型" width="120"></el-table-column>
        <el-table-column prop="autoSendOut" :formatter="formatDefault" label="自动发货" width="80"></el-table-column>
        <el-table-column label="过期时间" :formatter="formatExpireTime"></el-table-column>
        <el-table-column label="操作">
            <template slot-scope="scope">
                <el-button-group>
                    <el-button size="mini" @click="configMall(scope.$index, scope.row)">配置</el-button>
                    <el-button size="mini" @click="doRefreshToken(scope.$index, scope.row)">刷新Token</el-button>
                    <el-button size="mini" @click="syncOrder(scope.$index, scope.row)">同步订单</el-button>
                    <a :href="oauthUrl" target="_blank" class="el-button el-button--mini">重新授权</a>
                </el-button-group>
            </template>
        </el-table-column>
    </el-table>

    <!--工具条-->
    <el-col :span="24" class="toolbar">
        <el-pagination layout="total, sizes, prev, pager, next, jumper" :page-sizes="[50,100, 200, 300, 400]" @current-change="handleCurrentChange" @size-change="handleSizeChange" :page-size="50" :total="total" style="float:right;"></el-pagination>
    </el-col>
    <el-drawer title="请选择日期" :visible.sync="drawer">
        <syncOrder @onCancel="onCancel" @onSyncubmit="onSyncubmit"></syncOrder>
    </el-drawer>
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
import syncOrder from '../../components/synnOrder';
export default {
    components: {
        syncOrder
    },
    data() {
        return {
            filters: {
                mallName : "",
                ownerId  : "",
                ownerName: "",
                pageIndex: 0,
                pageSize : 50,
                valid    : -1
            },
            options: [{
                label: '全部',
                value: -1
            }, {
                label: '无效',
                value: 0
            }, {
                label: '有效',
                value: 1
            }],
            syncTimeArray    : [],
            timeNow          : new Date(),
            users            : [],
            total            : 0,
            page             : 1,
            drawer           : false,
            listLoading      : false,
            multipleSelection: [],
            oauthUrl         : ''
        };
    },
    methods: {
        handleSelectionChange(val) {
            this.multipleSelection = val;

        },
        configMall(index, row) {
            this.$router.push(`/mall/config/${row.id}`);
        },
        onCancel() {
            this.drawer = false;
        },
        onSyncubmit(time, call) {
            var $self = this;
            this.syncOrderRequest(time, function () {
                setTimeout(() => {
                    (typeof (call) == "function") && call();
                    $self.drawer = false;
                }, 500);
            });
        },
        onSyncOrder(cmd) {
            if (this.multipleSelection.length == 0) {
                this.msgError("没有选中任何店铺");
                return;
            }
            if (cmd == "other") {
                this.drawer = true;
                return;
            }
            this.syncOrderRequest(cmd);

        },
        syncOrderRequest(time, call) {
            var ids = [];
            for (var index in this.multipleSelection) {
                ids.push(this.multipleSelection[index].id);
            }
            let param = {
                mallIds  : ids.join(","),
                startTime: time + " 00:00:00",
                endTime  : time + " 23:59:59",
            };
            this.doGet("/mall/batchsyncorder", param, {
                success(resp) {
                    return true;
                },
                any() {
                    (typeof (call) == "function") && call();
                }
            });
        },
        formatExpireTime(val) {
            var expireTime  = new Date(val.expireTime);
            var millisecond = expireTime.getTime() - this.timeNow.getTime();
            if (millisecond < 0) {
                return "已过期";
            }
            return util.formatDate.timeShow(millisecond);
        },

        handleCurrentChange(val) {
            this.filters.pageIndex = val;
            this.doSearch();
        },
        handleSizeChange(val) {
            this.filters.pageSize = val;
            this.doSearch();
        },
        async syncOrder(index, model) {
            this.doGet("/mall/syncOrder", {
                id: model.id
            }, {
                success() {
                    return true;
                }
            });

        },
        async doRefreshToken(index, model) {
            let $self = this;
            this.doGet("/oauth/refresh", {
                mallId: model.ownerId
            }, {
                success() {
                    $self.doSearch();
                    return true;
                }
            });
        },
        //获取用户列表
        async doSearch() {
            this.timeNow     = new Date();
            this.listLoading = true;
            try {
                let resp       = await this.doGetSync("/mall/list", this.filters);
                    this.users = resp.data;
                    this.total = resp.total;
            } catch (e) {} finally {
                this.listLoading = false;
            }
        },
        initSyncTime() {
            this.syncTimeArray.push({
                label: '今天',
                value: this.getDay(0)
            });
            this.syncTimeArray.push({
                label: '昨天',
                value: this.getDay(-1)
            });
            this.syncTimeArray.push({
                label: '前天',
                value: this.getDay(-2)
            });
            for (var i = -3; i > -7; i--) {
                var day = this.getDay(i);
                this.syncTimeArray.push({
                    label: day,
                    value: day
                });
            }
            this.syncTimeArray.push({
                label: "其它日期",
                value: "other"
            });
        },
        getDay(day) {
            function doHandleMonth(month) {
                var m = month;
                if (month.toString().length == 1) {
                    m = "0" + month;
                }
                return m;
            }

            var today                  = new Date();
            var targetday_milliseconds = today.getTime() + 1000 * 60 * 60 * 24 * day;
            today.setTime(targetday_milliseconds); //注意，这行是关键代码
            var tYear  = today.getFullYear();
            var tMonth = today.getMonth();
            var tDate  = today.getDate();
                tMonth = doHandleMonth(tMonth + 1);
                tDate  = doHandleMonth(tDate);
            return tYear + "-" + tMonth + "-" + tDate;
        }

    },
    mounted() {
        this.oauthUrl = `/api/oauth/authorize?url=/api/oauth/code&state=${escape(location.origin+"/bind")}`
        this.initSyncTime();
        this.doSearch();
    }
};
</script>

<style scoped>
</style>
