<template>
  <div>
    <comment-input label="comment" @send="(c) => $emit('send', c)"/>

    <v-divider class="my-5" />

    <comment v-for="comment in comments" :comment="comment" :key="comment.id"/>

  </div>
</template>

<script>
  import CommentInput from "./comment-input";
  import Comment from "./comment";

  export default {
    name: "comment-section",
    components: {Comment, CommentInput},
    props: {
      comments: {
        required: true,
        type: Array,
      }
    }
  }
</script>

<style scoped>

</style>
