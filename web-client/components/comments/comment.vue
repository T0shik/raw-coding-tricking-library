<template>
  <div>
    <comment-body :comment="comment"
                  :parent-id="comment.id"
                  :parent-type="commentParentType"
                  @comment-created="appendComment"
                  @load-replies="loadReplies"/>
    <div class="ml-5">
      <comment-body v-for="c in comments"
                    :comment="c"
                    :parent-id="comment.id"
                    :parent-type="commentParentType"
                    @comment-created="appendComment"
                    :key="`reply-${c.id}`"/>
    </div>
  </div>
</template>

<script>
import CommentBody from "./comment-body";
import {COMMENT_PARENT_TYPE, container} from "@/components/comments/_shared";

export default {
  name: "comment",
  components: {CommentBody},
  mixins: [container],
  props: {
    comment: {
      required: true,
      type: Object,
    }
  },
  methods: {
    loadReplies() {
      return this.$axios.$get(`/api/comments/${this.comment.id}/replies`)
        .then((replies) => this.comments = replies)
    }
  },
  computed: {
    commentParentType() {
      return COMMENT_PARENT_TYPE.COMMENT
    }
  }
}
</script>

<style scoped>

</style>
