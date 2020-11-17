<template>
  <v-list>
    <v-list-item v-for="difficulty in difficulties" :key="`difficulty-history-${difficulty.id}`">
      <v-list-item-title>{{ difficulty.name }}</v-list-item-title>
    </v-list-item>
  </v-list>
</template>

<script>
export default {
  name: "history",
  data: () => ({
    difficulties: []
  }),
  fetch() {
    return this.$axios.$get(`/api/difficulties/${this.$route.params.difficulty}/history`)
      .then(difficulties => this.difficulties = difficulties)
  }
}
</script>

<style scoped>

</style>
