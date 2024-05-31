<template>
<section>
    <!--工具条-->
    <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
        <el-form :inline="true" :model="filters">
            <el-form-item>
                <el-input v-model="filters.name" placeholder="发货人姓名"></el-input>
            </el-form-item>
            <el-form-item>
                <el-input v-model="filters.mobile" placeholder="发货人电话"></el-input>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" v-on:click="doSearch">查询</el-button>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="addAddress">添加地址</el-button>
            </el-form-item>
        </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="list" highlight-current-row v-loading="listLoading" style="width: 100%;">
        <el-table-column prop="id" label="id" width="60"></el-table-column>
        <el-table-column prop="name" label="姓名" width="100"></el-table-column>
        <el-table-column prop="mobile" label="电话" width="120"></el-table-column>
        <el-table-column prop="province" label="省" width="100"></el-table-column>
        <el-table-column prop="city" label="市" width="100"></el-table-column>
        <el-table-column prop="town" label="区" width="100"></el-table-column>
        <el-table-column prop="detail" label="地址"></el-table-column>
        <el-table-column prop="default" label="默认地址" :formatter="formatDefault" width="120"></el-table-column>
        <!-- <el-table-column label="过期时间" :formatter="formatExpireTime"></el-table-column> -->
        <el-table-column label="操作">
            <template slot-scope="scope">
                <el-button-group>
                    <el-button size="mini" @click="edit(scope.$index, scope.row)">编辑</el-button>
                    <el-popconfirm title="确定要删除该地址吗？" @onConfirm="del(scope.$index, scope.row)">
                        <el-button type="danger" size="mini" slot="reference">删除</el-button>
                    </el-popconfirm>
                    <el-button v-if="!scope.row.default" type="primary" size="mini" @click="setDefault(scope.$index, scope.row)">设为默认</el-button>
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
import axios from "axios";
export default {
    data() {
        return {
            filters: {
                name  : "",
                mobile: ""
            },
            list       : [],
            total      : 0,
            page       : 1,
            listLoading: false
        };
    },
    methods: {
        async del($index, $row) {
            let $self = this;
            $self.doGet(
                "/address/delete", {
                    id: $row.id
                }, {
                    success: function () {
                        $self.doSearch();
                        return true;
                    }
                }
            );
        },
        edit($index, $row) {
            this.$router.push(`/address/edit/${$row.id}`);
        },
        addAddress() {
            this.$router.push("/address/add");
        },
        async setDefault($index, $row) {
            let $self = this;
            $self.doGet(
                "/address/setdefault", {
                    id: $row.id
                }, {
                    success() {
                        $self.doSearch();
                        return true;
                    }
                }
            );
        },
        handleCurrentChange(val) {
            this.page = val;
            this.doSearch();
        },
        async doSearch() {
                this.timeNow = new Date();
            let para         = {
                pageIndex: this.page,
                pageSize : 15,
                name     : this.filters.name,
                mobile   : this.filters.mobile
            };
            this.listLoading = true;
            try {
                let resp       = await this.doGetSync("/address/list", para);
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

<style scoped></style>
