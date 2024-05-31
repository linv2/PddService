<template>
<div class="el-alert el-alert--error is-light" v-show="!this.$store.state.socket.isConnected">
    <i class="el-alert__icon el-icon-error"></i>
    <div class="el-alert__content">
        <span class="el-alert__title">
            菜鸟打印组件未启动，请先启动打印组件。<a href="http://cloudprint.cainiao.com/cloudprint/client/CNPrintSetup.exe" target="_blank">点我下载</a>
        </span>
    </div>
</div>
</template>

<style scoped>
.el-alert {
    margin: 5px;
}
</style>

<script>
import util from '../common/js/util';

function getRequestObject(cmd, uuid) {
    var request           = new Object();
        request.requestID = uuid || util.getUUID(8, 16);
        request.version   = "1.0";
        request.cmd       = cmd;
    return request;
}
export default {
    data() {
        return {
            callArray  : [],
            printNotity: function () {}
        };
    },
    watch: {
        "$store.state.socket.message": function (event) {
            let data = event.data;
            console.log("revice data=" + data);
            var response = eval("(" + data + ")");
            let call     = this.callArray[response.requestID];
            call && call(response);
            delete this.callArray[response.requestID];
            if (response.cmd === "notifyPrintResult") {
                this.$store.dispatch("notifyPrintResult", response);
            }
        }
    },
    methods: {
        notity(fun) {

        },
        sendMessage(data) {
            if (!this.$store.state.socket.isConnected) {
                this.$message({
                    message : "菜鸟打印组件未启动，请先启动打印组件。",
                    type    : "error",
                    duration: 2000
                });
                return;
            }
            let sendData = typeof (data) == 'object' ? JSON.stringify(data) : data;
            console.log("send data=" + sendData);
            this.$store.dispatch("sendMessage", sendData);
        },
        async getPrinters(timeOut) {
            let uuid    = util.getUUID(8, 16);
            var request = getRequestObject("getPrinters", uuid);
            this.sendMessage(request);
            return this.resVal(uuid, timeOut);
        },
        async printDocument(printerName, printDataList, timeOut) {
            let uuid                 = util.getUUID(8, 16);
            var request              = getRequestObject("print", uuid);
                request.task         = new Object();
                request.task.taskID  = uuid;
                request.task.preview = false;
                request.notifyType   = ["print"];
            //request.task.previewType = "pdf"; 
                request.task.printer = printerName;
            var documents            = new Array();
            for (var index in printDataList) {
                let item           = printDataList[index];
                var doc            = new Object();
                    doc.documentID = item.trackingNumber;
                    doc.contents   = [];
                doc.contents.push(item.printData)
                documents.push(doc);
            }

            request.task.documents = documents;
            this.sendMessage(request);
            return this.resVal(uuid);
        },
        resVal(uuid, timeOut) {
            let $self = this;
            return new Promise(function (resolve, reject) {
                $self.callArray[uuid] = resolve;
                setTimeout(() => {
                    if ($self.callArray[uuid]) {
                        reject({
                            message: "等待超时"
                        });
                    }
                    delete $self.callArray[uuid];
                }, timeOut || 1500);
            });
        }
    },
    mounted() {
        this.$store.dispatch("transportObj", this);
    },
}
</script>
