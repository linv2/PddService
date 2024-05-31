<template>
<section>
    <!--工具条-->
    <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
        <el-form :inline="true" :model="filters">
            <el-form-item>
                <el-input v-model="filters.name" placeholder="模板名称"></el-input>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" v-on:click="doSearch">查询</el-button>

                <el-button type="primary" @click.native.prevent="addTemplate">添加模板</el-button>
            </el-form-item>
        </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="list" highlight-current-row v-loading="listLoading" style="width: 100%;">
        <el-table-column prop="id" label="id" width="60"></el-table-column>
        <el-table-column prop="name" label="模板名称" width="250"></el-table-column>
        <el-table-column prop="express.name" label="快递" width="120"></el-table-column>
        <el-table-column prop="size" label="面单尺寸" width="120"></el-table-column>
        <el-table-column prop="createdTime" label="创建时间" width="200"></el-table-column>
        <el-table-column prop="default" label="默认模板" :formatter="formatDefault" width="200"></el-table-column>
        <el-table-column label="操作">
            <template slot-scope="scope">
                <el-button-group>
                    <el-popconfirm title="确定要删除该地址吗？" @onConfirm="del(scope.$index, scope.row)">

                        <el-button type="danger" size="mini" slot="reference">删除</el-button>
                    </el-popconfirm>
                    <el-button v-if="!scope.row.default" size="mini" @click="setDefault(scope.$index, scope.row)">设为默认</el-button>
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
                name: ""
            },
            list       : [],
            total      : 0,
            page       : 1,
            listLoading: false
        };
    },
    methods: {
        async setDefault($index, $row) {
            let $self = this;
            $self.doGet(
                "/express/setdefault", {
                    id: $row.id
                }, {
                    success: function () {
                        $self.doSearch();
                        return true;
                    }
                }
            );
        },
        async del($index, $row) {
            let $self = this;
            $self.doGet(
                "/express/deletetemplate", {
                    id: $row.id
                }, {
                    success: function () {
                        $self.doSearch();
                        return true;
                    }
                }
            );
        },
        async addTemplate() {
            this.$router.push("/express/list");
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
                name     : this.filters.name
            };
            this.listLoading = true;
            try {
                let resp       = await this.doGetSync("/express/templatelist", para);
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
