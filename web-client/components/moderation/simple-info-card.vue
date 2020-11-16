<template>
  <v-card>
    <v-card-title>{{ payload.name }}</v-card-title>
    <v-card-text>{{ payload.description }}</v-card-text>
  </v-card>
</template>

<script>
export default {
  name: "simple-info-card",
  props: {
    payload: {
      required: true,
      type: Object
    }
  }
}
</script>

<style scoped>

</style>
