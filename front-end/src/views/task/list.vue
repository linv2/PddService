<template>
<section>
    <!--工具条-->
    <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
        <el-form :inline="true" :model="filters">
            <el-form-item>
                <el-input v-model="filters.taskKey" placeholder="任务Id" clearable></el-input>
            </el-form-item>
            <el-form-item>
                <el-select v-model="filters.taskStatus" placeholder="任务状态" class="w120" clearable>
                    <el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value">
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" v-on:click="doSearch">查询</el-button>

            </el-form-item>
        </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="list" highlight-current-row v-loading="listLoading" style="width: 100%;">
        <el-table-column prop="id" label="id" width="60"></el-table-column>
        <el-table-column prop="taskKey" label="任务Id" width="270"></el-table-column>
        <el-table-column :formatter="formatTaskType" prop="taskType" label="任务类型" width="120"></el-table-column>
        <el-table-column prop="createdTime" label="创建时间" width="160"></el-table-column>
        <el-table-column :formatter="formatTaskStatus" prop="taskStatus" label="任务状态" width="60"></el-table-column>
        <el-table-column prop="completeTime" label="完成时间" width="160"></el-table-column>
        <el-table-column label="操作">
            <template slot-scope="scope">
                <el-button-group>
                    <a :href="scope.row.completeParam" target="_blank" v-if="scope.row.taskType==1&&scope.row.taskStatus==1" class="el-button el-button--mini">下载文件</a>
                </el-button-group>
            </template>
        </el-table-column>
    </el-table>

    <!--工具条-->
    <el-col :span="24" class="toolbar">
        <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="20" :total="total" style="float:right;"></el-pagination>
    </el-col>
</section>
</template>

<script>
export default {
    data() {
        return {
            filters: {
                taskKey   : "",
                pageIndex : 1,
                pageSize  : 20,
                taskStatus: -1
            },
            options: [{
                label: '全部',
                value: -1
            }, {
                label: '执行中',
                value: 0
            }, {
                label: '成功',
                value: 1
            }, {
                label: '失败',
                value: 2
            }],
            list       : [],
            total      : 0,
            listLoading: false
        };
    },
    methods: {
        formatTaskType(val) {
            val = (arguments.length == 4 ? arguments[2] : val);
            switch (val) {
                case 1: {
                    return "导出订单";
                }
                default: 
                    return "-";
            }
        },
        formatTaskStatus(val) {
            val = (arguments.length == 4 ? arguments[2] : val);
            switch (val) {
                case 0: {
                    return "执行中";
                }
                case 1: {
                    return "成功";
                }
                case 2: {
                    return "失败";
                }
                default: 
                    return "-";
            }
        },
        handleCurrentChange(val) {
            this.filters.pageIndex = val;
            this.doSearch();
        },
        async doSearch() {
            this.listLoading = true;
            try {
                let resp       = await this.doGetSync("/task/list", this.filters);
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
