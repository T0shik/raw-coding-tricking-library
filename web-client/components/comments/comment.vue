<template>
  <div>
    <comment-body :comment="comment"
                  :parent-id="comment.id"
                  :parent-type="commentParentType"
                  @comment-created="(c) => content.push(c)"
                  @[loadRepliesEvent]="loadContent"/>
    <div class="ml-5">
      <comment-body v-for="c in content"
                    :comment="c"
                    :parent-id="comment.id"
                    :parent-type="commentParentType"
                    @comment-created="(x) => content.push(x)"
                    :key="`reply-${c.id}`"/>
      <div class="d-flex justify-center" v-if="content.length > 0 && !finished">
        <v-btn outlined small @click="loadContent">load more</v-btn>
      </div>
    </div>
  </div>
</template>

<script>
import CommentBody from "./comment-body";
import {COMMENT_PARENT_TYPE, container} from "@/components/comments/_shared";
import {feed} from "@/components/feed";

export default {
  name: "comment",
  components: {CommentBody},
  mixins: [feed('first')],
  props: {
    comment: {
      required: true,
      type: Object,
    }
  },
  methods: {
    getContentUrl() {
      return `/api/comments/${this.comment.id}/${this.commentParentType}${this.query}`
    }
  },
  computed: {
    commentParentType() {
      return COMMENT_PARENT_TYPE.COMMENT
    },
    loadRepliesEvent() {
      return this.content.length === 0 ? 'load-replies' : ''
    }
  }
}
</script>

<style scoped>

</style>
