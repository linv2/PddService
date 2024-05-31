<template>
<el-form ref="ruleForm" :model="form" :rules="rules" label-width="80px" @submit.prevent="onSubmit" style="margin:20px;width:60%;min-width:600px;">
    <el-form-item label="面单地址">
        <el-select v-model="selectedAddress" clear clearable placeholder="面单地址" @change="handleAddrChange" v-if="this.addressList.length>0">
            <el-option v-for="item in addressList" :key="item.province+item.city+item.district+item.detail" :label="item.province+item.city+item.district+item.detail" :value="item">
            </el-option>
        </el-select>
    </el-form-item>
    <el-form-item label="发货姓名" prop="name">
        <el-input v-model="form.name"></el-input>
    </el-form-item>
    <el-form-item label="发货电话" prop="mobile">
        <el-input v-model="form.mobile"></el-input>
    </el-form-item>
    <el-form-item label="城市" prop="town">
        <el-cascader :options="options" v-model="selectedOptions" @change="handleChange"></el-cascader>
    </el-form-item>
    <el-form-item label="详细地址" prop="detail">
        <el-input v-model="form.detail" type="textarea"></el-input>
    </el-form-item>
    <el-form-item>
        <el-button type="primary" :loading="loading" @click="onValid('ruleForm')">立即创建</el-button>
        <el-button @click.native.prevent="onCancel">返回</el-button>
    </el-form-item>
</el-form>
</template>

<script>
import axios from "axios";
import {
    regionData,
    TextToCode,
    CodeToText
} from "element-china-area-data";
export default {
    data() {
        return {
            form: {
                name    : "",
                mobile  : "",
                province: "",
                city    : "",
                town    : "",
                detail  : ""
            },
            rules: {
                name: [{
                        required: true,
                        message : "请输入发货人姓名",
                        trigger : "blur"
                    },
                    {
                        min    : 1,
                        max    : 20,
                        message: "长度错误",
                        trigger: "blur"
                    }
                ],
                mobile: [{
                        required: true,
                        message : "请输入发货人电话",
                        trigger : "blur"
                    },
                    {
                        min    : 11,
                        max    : 11,
                        message: "格式错误",
                        trigger: "blur"
                    }
                ],
                detail: [{
                        required: true,
                        message : "请输入详细地址",
                        trigger : "blur"
                    },
                    {
                        min    : 5,
                        max    : 40,
                        message: "长度错误",
                        trigger: "blur"
                    }
                ],
                town: [{
                    required: true,
                    message : "请选择城市数据"
                }]
            },
            options        : regionData,
            selectedOptions: [],
            loading        : false,
            selectedAddress: undefined,
            addressList    : [],
        };
    },
    methods: {
        handleChange(value) {
            this.form.province = CodeToText[value[0]];
            this.form.city     = CodeToText[value[1]];
            this.form.town     = CodeToText[value[2]];
        },
        onValid(formName) {
            var $self = this;
            this.$refs[formName].validate(valid => {
                if (valid) {
                    $self.onSubmit();
                } else {
                    return false;
                }
            });
        },
        async onSubmit() {
            let self         = this;
                self.loading = true;

            self.doPost("/address/add", self.form, {
                success: function () {
                    self.$router.push({
                        path: `/address/list/`
                    });
                },
                any: function () {
                    self.loading = false;
                }
            });
        },
        onCancel() {
            this.$router.push("/address/list");
        },
        handleAddrChange(val) {
            if (!val) {
                return;
            }
            var townCode = 
                TextToCode[val.province][val.city][val.district].code;
            var cityCode             = TextToCode[val.province][val.city].code;
            var provinceCode         = TextToCode[val.province].code;
                this.selectedOptions = [provinceCode, cityCode, townCode];
                this.form.detail     = val.detail;
                this.form.province   = val.province;
                this.form.city       = val.city;
                this.form.town       = val.district
        },
        loadAddress() {
            let $self = this;
            this.doGet("/cainiao/defaultquerywaybill", {}, {
                success(res) {
                    if (res.code == 200) {
                        $self.addressList = res.data.shippAddressCols;
                        //   $self.addressList.unshift($self.selectedAddress);
                    }
                }
            });
        }
    },
    mounted() {
        this.loadAddress();
    }
};
</script>
