<template>
  <div>
    <v-btn :to="`/moderation/${i.id}`" :key="i.id" v-for="i in items">
      {{i.target}}
    </v-btn>
  </div>
</template>

<script>
import {guard, GUARD_LEVEL} from "@/components/auth/auth-mixins";

  export default {
    mixins: [guard(GUARD_LEVEL.AUTH)],
    data: () => ({
      items: []
    }),
    async fetch() {
      if(process.server) return;

      this.items = await this.$axios.$get("/api/moderation-items")
    }
  }
</script>

<style scoped>

</style>
