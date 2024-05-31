<template>
<div></div>
</template>

<script>
export default {
    data() {
        return {
            taskList: []
        };
    },
    methods: {
        addJob(taskKey) {
            if (taskKey) {
                this.taskList.push(taskKey);
            }
        },
        async queryJob() {
            if (this.taskList.length == 0) {
                return;
            }
            let resp        = await this.doPostSync("/task/sync", this.taskList);
            let respData    = resp.data;
            var newTaskList = [];
            for (var key in respData) {
                let val = respData[key];
                if (val.taskStatus === 0) {
                    newTaskList.push(val.taskKey);
                } else if (val.taskStatus === 1) {
                    if (val.taskType == 1) {
                        this.showDownLoadNotity(val, val.completeParam)

                    } else {
                        this.$notify({
                            title   : '任务完成',
                            message : `任务Id为${val.id}的任务执行完成`,
                            type    : 'success',
                            duration: 0
                        });
                    }
                } else if (val.taskStatus == 2) {
                    this.$notify.error({
                        title   : '任务异常',
                        message : `任务序号为${val.id}的任务执行失败`,
                        duration: 0
                    });
                }
            }
            this.taskList = newTaskList;
        },
        showDownLoadNotity(model, url) {
            this.$notify({
                title                   : '任务完成',
                dangerouslyUseHTMLString: true,
                message                 : `任务Id为${model.id}的任务执行完成<br /><a href="${url}" target="_blank">点击下载</a>`,
                type                    : 'success',
                duration                : 0
            });
        }
    },
    mounted() {
        setInterval(this.queryJob, 3000);
    }
}
</script>
