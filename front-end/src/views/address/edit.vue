<template>
<el-form ref="ruleForm" :model="form" :rules="rules" label-width="80px" @submit.prevent="onSubmit" style="margin:20px;width:60%;min-width:600px;">
    <el-form-item label="发货姓名" prop="name">
        <el-input v-model="form.name"></el-input>
    </el-form-item>
    <el-form-item label="发货电话" prop="mobile">
        <el-input v-model="form.mobile"></el-input>
    </el-form-item>
    <el-form-item label="城市" prop="town" v-if="cityRender">
        <el-cascader :options="options" v-model="selectedOptions" @change="handleChange"></el-cascader>
    </el-form-item>
    <el-form-item label="详细地址" prop="detail">
        <el-input v-model="form.detail" type="textarea"></el-input>
    </el-form-item>
    <el-form-item>
        <el-button type="primary" :loading="loading" @click="onValid('ruleForm')">保存</el-button>
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
                name: "",
                mobile: "",
                province: "",
                city: "",
                town: "",
                detail: ""
            },
            rules: {
                name: [{
                        required: true,
                        message: "请输入发货人姓名",
                        trigger: "blur"
                    },
                    {
                        min: 1,
                        max: 20,
                        message: "长度错误",
                        trigger: "blur"
                    }
                ],
                mobile: [{
                        required: true,
                        message: "请输入发货人电话",
                        trigger: "blur"
                    },
                    {
                        min: 11,
                        max: 11,
                        message: "格式错误",
                        trigger: "blur"
                    }
                ],
                detail: [{
                        required: true,
                        message: "请输入详细地址",
                        trigger: "blur"
                    },
                    {
                        min: 5,
                        max: 40,
                        message: "长度错误",
                        trigger: "blur"
                    }
                ],
                town: [{
                    required: true,
                    message: "请选择城市数据"
                }]
            },
            options: regionData,
            selectedOptions: [],
            cityRender: false,
            loading: false
        };
    },
    methods: {
        handleChange(value) {
            this.form.province = CodeToText[value[0]];
            this.form.city = CodeToText[value[1]];
            this.form.town = CodeToText[value[2]];
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
            let self = this;
            self.loading = true;

            self.doPost("/address/edit", self.form, {
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
        }
    },
    mounted() {
        let self = this;
        let id = this.$route.params.id;
        this.doGet(
            "/address/get", {
                id
            }, {
                success(res) {
                    for (var k in res.data) {
                        self.$set(self.form, k, res.data[k]);
                    }
                    var townCode =
                        TextToCode[self.form.province][self.form.city][self.form.town].code;
                    var cityCode = TextToCode[self.form.province][self.form.city].code;
                    var provinceCode = TextToCode[self.form.province].code;
                    self.selectedOptions.push(provinceCode, cityCode, townCode);
                    self.cityRender = true;
                },
                fail() {
                    self.$message({
                        message: "系统错误，数据不存在",
                        type: "error"
                    });
                }
            }
        );
    }
};
</script>
