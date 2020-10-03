<template>
  <div class="d-flex align-center pa-2">
    <v-avatar :size="size">
      <img :src="imageUrl" />
    </v-avatar>
    <div class="ml-3" v-if="username">
      {{username}}
    </div>
    <v-spacer />
  </div>
</template>

<script>
export default {
  name: "user-header",
  props: {
    imageUrl: {
      required: false,
      type: String,
    },
    username: {
      required: false,
      type: String,
    },
    size: {
      required: false,
      type: String,
      default: '42',
    }
  }
}
</script>

<style scoped>

</style>
