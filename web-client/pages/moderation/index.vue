<template>
  <div>
    <v-btn :to="`/moderation/${i.id}/${i.type}/${i.target}`" :key="i.id" v-for="i in items">
      {{i.target}}
    </v-btn>
  </div>
</template>

<script>
  export default {
    data: () => ({
      items: []
    }),
    async fetch() {
      this.items = await this.$axios.$get("/api/moderation-items")
    }
  }
</script>

<style scoped>

</style>
