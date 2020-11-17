<template>
  <v-list>
    <v-list-item v-for="category in categories" :key="`category-history-${category.id}`">
      <v-list-item-title>{{ category.name }}</v-list-item-title>
    </v-list-item>
  </v-list>
</template>

<script>
export default {
  name: "history",
  data: () => ({
    categories: []
  }),
  fetch() {
    return this.$axios.$get(`/api/categories/${this.$route.params.category}/history`)
      .then(categories => this.categories = categories)
  }
}
</script>

<style scoped>

</style>
