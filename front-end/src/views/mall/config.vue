<template>
<el-form :model="form" label-width="140px" @submit.prevent="onSubmit" style="margin:20px;width:60%;min-width:600px;">
    <el-form-item label="发货地址">
        <el-select v-model="form.addressId" clear clearable placeholder="发货地址" v-if="this.addressList.length>0">
            <el-option v-for="item in addressList" :key="item.id" :label="item.province+item.city+item.town+item.detail" :value="item.id">
            </el-option>
        </el-select>
        <el-tag type="danger" class="tag">如果发货地址为空，则读取地址列表的默认选项</el-tag>

    </el-form-item>
    <el-form-item label="面单模板">
        <el-select v-model="form.printTemplateId" clear clearable placeholder="面单地址" v-if="this.addressList.length>0">
            <el-option v-for="item in printTemplateList" :key="item.id" :label="item.name" :value="item.id">
            </el-option>
        </el-select>
        <el-tag type="danger" class="tag">如果面单模板为空，则读取面单模板列表的默认选项</el-tag>
    </el-form-item>

    <el-form-item label="自动发货">
        <el-switch v-model="form.autoSendOut">
        </el-switch>
    </el-form-item>
    <el-form-item label="自动发货时间段" v-show="form.autoSendOut">
        <el-time-picker is-range v-model="form.autoSendOutTimeRange" range-separator="-" start-placeholder="开始时间" end-placeholder="结束时间" placeholder="选择时间范围" value-format="HHmmss">
        </el-time-picker>
    </el-form-item>

    <el-form-item label="自动通知平台发货">
        <el-switch v-model="form.autoNotity">
        </el-switch>
    </el-form-item>
    <el-form-item label="未发货自动同意退款">
        <el-switch v-model="form.autoCancel">
        </el-switch>
    </el-form-item>
    <el-form-item>
        <el-button type="primary" @click="onSubmit">保存</el-button>
        <el-button @click.native.prevent="onCancel">返回</el-button>
    </el-form-item>
</el-form>
</template>

<style scoped>
.tag {
    margin-left: 10px;
}
</style>

<script>
export default {
    data() {
        return {
            form: {
                id                  : 0,
                addressId           : null,
                printTemplateId     : null,
                autoSendOut         : false,
                autoCancel          : false,
                autoNotity          : false,
                autoSendOutTimeRange: ["000000", "000000"]
            },
            mallInfo         : {},
            addressList      : [],
            printTemplateList: []
        };
    },
    methods: {
        async loadParam() {
            let id   = this.$route.params.id;
            let resp = await this.doGetSync("/mall/get", {
                id
            });
            this.mallInfo = resp.data;
            for (var k in resp.data) {
                this.$set(this.form, k, resp.data[k]);
            }
            this.form.autoSendOutTimeRange = [this.formatTimePicker(this.mallInfo.autoSendOutStartTime), this.formatTimePicker(this.mallInfo.autoSendOutEndTime)];
        },
        formatTimePicker(val) {
            val = val.toString();
            while (val.length < 6) {
                val = "0" + val;
            }
            return val;
        },
        async onSubmit() {
            let id   = this.$route.params.id;
            let resp = await this.doPostSync("/mall/updateconfig", this.form);
            if (resp.code == 200) {
                this.msgSuccess(resp.message);
                this.$router.push("/mall/list");
            } else {
                this.msgError(resp.message);
            }
        },
        onCancel() {
            this.$router.push("/mall/list");
        },
        async loadAddress() {
            let resp = await this.doGetSync("/address/list", {
                pageSize: 30
            });
            this.addressList = resp.data;
        },
        async loadPrintTemplate() {
            let resp = await this.doGetSync("/express/templatelist", {
                pageSize: 30
            });
            this.printTemplateList = resp.data;
        }
    },
    mounted() {
        this.loadParam();
        this.loadAddress();
        this.loadPrintTemplate();
    },
}
</script>
