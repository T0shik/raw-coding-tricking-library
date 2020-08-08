<template>
  <div class="my-1">
    <div>
      <p class="mb-1" v-html="comment.htmlContent"></p>
      <v-btn small text v-if="!replying" @click="replying = true">reply</v-btn>
      <v-btn small text v-if="$listeners['load-replies']" @click="$emit('load-replies')">load replies</v-btn>
    </div>

    <comment-input v-if="replying" @send="(c) => $emit('send', c)" @cancel="replying = false" />

  </div>
</template>

<script>
  import CommentInput from "./comment-input";

  export default {
    name: "comment-body",
    components: {CommentInput},
    props: {
      comment: {
        required: true,
        type: Object,
      }
    },
    data: () => ({
      replying: false
    })
  }
</script>

<style scoped>

</style>
