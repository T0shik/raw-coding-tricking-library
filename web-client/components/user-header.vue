<template>
  <div class="d-flex align-center pa-2" :class="{'flex-row-reverse': reverse}">
    <v-avatar :size="size">
      <img v-if="imageUrl" :src="imageUrl" />
      <v-icon v-else>mdi-account</v-icon>
    </v-avatar>
    <div class="mx-3" v-if="username">
      {{username}}
    </div>
    <v-spacer />
    <div v-if="append">
      {{append}}
    </div>
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
    },
    append: {
      required: false,
      type: String,
      default: '',
    },
    reverse: {
      required: false,
      type: Boolean,
      default: false,
    }
  }
}
</script>

<style scoped>

</style>
