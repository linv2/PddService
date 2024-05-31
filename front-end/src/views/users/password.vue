<template>
<el-form ref="ruleForm" :model="form" :rules="rules" label-width="80px" style="margin:20px;">
    <el-form-item label="原始密码" prop="oldPassword">
        <el-input v-model="form.oldPassword" type="password"></el-input>
    </el-form-item>
    <el-form-item label="新密码" prop="newPassword">
        <el-input v-model="form.newPassword" type="password"></el-input>
    </el-form-item>
    <el-form-item label="确认密码" prop="newPassword2">
        <el-input v-model="form.newPassword2" type="password"></el-input>
    </el-form-item>
    <el-form-item>
        <el-button type="primary" :loading="logining" @click.native.prevent="onSubmit('ruleForm')">立即修改</el-button>
        <el-button  @click="doBack">返回</el-button>
    </el-form-item>
</el-form>
</template>

<script>
import axios from "axios";
export default {
    data() {
        var checkPassWord = (rule, value, callback) => {
            if (value === '') {
                callback(new Error('请再次输入密码'));
            } else if (value !== this.form.newPassword) {
                callback(new Error('两次输入密码不一致!'));
            } else {
                callback();
            }
        };
        return {
            form: {
                oldPassword : '',
                newPassword : '',
                newPassword2: ''
            },
            rules: {
                oldPassword: [{
                    required: true,
                    message : "旧密码不能为空",
                    trigger : "blur"
                }],
                newPassword: [{
                    required: true,
                    message : "新密码不能为空",
                    trigger : "blur"
                }],
                newPassword2: [{
                    required: true,
                    message : "确认密码不能为空",
                    trigger : "blur"
                }, {
                    validator: checkPassWord,
                    trigger  : 'blur'
                }]

            },
            logining: false
        };
    },
    methods: {
        doBack() {
            this.$router.back();
        },
        async onSubmit(formName) {
            let $self = this;
            $self.$refs[formName].validate(valid => {
                $self.doPost("/user/updatepassword", $self.form, {
                    success(res) {
                        $self.$alert("密码修改成功，点击确定重新登录", "提示", {
                            callback: function () {
                                window.location = "/user/logout?url=/login"
                            }
                        });
                        return false;
                    },
                    any() {
                        $self.logining = false;
                    }
                });
            });
        }
    },
    mounted() {}
};
</script>
