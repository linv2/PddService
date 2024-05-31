<script>
import axios from "axios";
const serviceUrl = window.serviceUrl || '';
export default {
    methods: {
        formatDefault(val) {
            return (arguments.length == 4 ? (arguments[2] ? "是" : "否") : (val ? "是" : "否"));
        },
        formatMoney(val) {
            return "￥" + (arguments.length == 4 ? arguments[2].toFixed(2) : val.toFixed(2));
        },
        msgShow(resp) {
            (resp.code == 200 ? this.msgSuccess : this.msgError)(resp.message);
        },
        msgSuccess(msg) {
            this.$message({
                message: msg,
                type: "success",
                duration: 1500
            });
        },
        msgError(msg) {
            this.$message({
                message: msg,
                type: "error",
                duration: 1500
            });
        },
        async doPost(url, param, conf) {
            try {
                let resp = await axios.post(serviceUrl + url, param);
                const respData = resp.data;
                if (respData.code != 200) {
                    if (typeof conf.fail !== "function" || !conf.fail(respData))
                        this.msgError(respData.message);
                    return;
                }
                if (typeof conf.success == "function" && conf.success(respData)) {
                    this.msgSuccess(respData.message);
                }
            } catch (e) {
                this.msgError(e.message);
            } finally {
                typeof conf.any == "function" && conf.any();
            }
        },
        async doGet(url, param, conf) {
            try {
                let resp = await axios.get(serviceUrl + url, {
                    params: param
                });
                const respData = resp.data;
                if (respData.code != 200) {
                    if (typeof conf.fail !== "function" || !conf.fail(respData))
                        this.msgError(respData.message);
                    return;
                } else {
                    if (typeof conf.success == "function" && conf.success(respData)) {
                        this.msgSuccess(respData.message);
                    }
                }
            } catch (e) {
                this.msgError(e.message);
            } finally {
                typeof conf.any == "function" && conf.any();
            }
        },
        doGetSync(url, param) {
            return new Promise(function (reslove, reject) {
                axios.get(serviceUrl + url, {
                        params: param
                    }).then(function (response) {
                        reslove(response.data);
                    })
                    .catch(function (error) {
                        reject({
                            message: error
                        });
                    });;
            });
        },
        doPostSync(url, param) {
            return new Promise(function (reslove, reject) {
                axios.post(serviceUrl + url, param).then(function (response) {
                        reslove(response.data);
                    })
                    .catch(function (error) {
                        reject({
                            message: error
                        });
                    });;
            });
        }
    }
};
</script>
