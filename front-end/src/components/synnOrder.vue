<template>
<div class="drawer_content">
    <el-form ref="form">
        <el-form-item label="同步日期" label-width="80px">
            <el-date-picker v-model="form.syncTime" type="date" placeholder="选择日期" format="yyyy 年 MM 月 dd 日" value-format="yyyy-MM-dd">
            </el-date-picker>
        </el-form-item>
    </el-form>
    <div class="drawer_footer">
        <el-button type="primary" @click="onSyncubmit" :loading="loading">{{ loading ? '提交中 ...' : '开始同步' }}</el-button>
        <el-button @click="onCancel">取 消</el-button>
    </div>
</div>
</template>

<style scoped>
.drawer_content {
    margin: 0px 10px;
    display: flex;
    flex-direction: column;
    height: 95%;
}

.drawer_content form {
    display: block;
    flex: 1;
    margin-top: 0em;
}

.scroll {
    overflow-y: scroll;
    height: 20%;
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
    data() {
        return {
            loading: false,
            dataSource: [],
            form: {
                syncTime: ''
            }
        };
    },

    methods: {
        onCancel() {
            this.$emit("onCancel");
        },
        onSyncubmit() {
            var $self = this;
            $self.loading = true;
            this.$emit("onSyncubmit", this.form.syncTime, function () {
                $self.loading = false;

            });
        }
    },
    mounted: function () {}
};
</script>
