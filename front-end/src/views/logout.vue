<template>
<div></div>
</template>

<script>
import axios from "axios";
export default {
    data() {
        return {};
    },
    async mounted() {
        let res = await this.doGetSync("/oauth/logout");
        this.$router.push("/login");
    }
}
</script>
