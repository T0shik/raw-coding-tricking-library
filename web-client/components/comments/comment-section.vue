<template>
  <div>
    <comment-input label="comment"
                   :parent-id="parentId"
                   :parent-type="parentType"
                   @comment-created="prependComment"/>

    <v-divider class="my-5"/>

    <comment v-for="comment in comments" :comment="comment" :key="comment.id"/>

  </div>
</template>

<script>
import CommentInput from "./comment-input";
import Comment from "./comment";
import {COMMENT_PARENT_TYPE, configurable, container} from "@/components/comments/_shared";

export default {
  name: "comment-section",
  components: {Comment, CommentInput},
  mixins: [configurable, container],
  created() {
    return this.$axios.$get(this.endpoint)
      .then((comments) => comments.forEach(x => this.comments.push(x)))
  },
  computed: {
    endpoint() {
      if (this.parentType === COMMENT_PARENT_TYPE.MODERATION_ITEM) {
        return `/api/moderation-items/${this.parentId}/comments`
      }
      if (this.parentType === COMMENT_PARENT_TYPE.SUBMISSION) {
        return `/api/submissions/${this.parentId}/comments`
      }
    }
  }
}
</script>

<style scoped>

</style>
