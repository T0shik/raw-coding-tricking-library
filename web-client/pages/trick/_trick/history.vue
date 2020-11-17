<template>
  <v-list>
    <v-list-item v-for="trick in tricks" :key="`trick-history-${trick.id}`">
      <v-list-item-title>{{ trick.name }}</v-list-item-title>
    </v-list-item>
  </v-list>
</template>

<script>
export default {
  name: "history",
  data: () => ({
    tricks: []
  }),
  fetch() {
    return this.$axios.$get(`/api/tricks/${this.$route.params.trick}/history`)
      .then(tricks => this.tricks = tricks)
  }
}
</script>

<style scoped>

</style>
