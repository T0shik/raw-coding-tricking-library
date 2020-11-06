<template>
  <div class="my-1">
    <div>
      <v-sheet class="d-flex align-center mb-1" rounded color="grey darken-3">
        <user-header class="pa-2" :username="comment.user.username" :image-url="comment.user.image" size="28"/>
        <div v-html="comment.htmlContent"></div>
      </v-sheet>
      <v-btn small text v-if="!replying" @click="replying = true">reply</v-btn>
      <v-btn small text v-if="$listeners['load-replies']" @click="$emit('load-replies')">load replies</v-btn>
    </div>

    <comment-input label="Reply"
                   v-if="replying"
                   :parent-id="parentId"
                   :parent-type="parentType"
                   @comment-created="emitComment"
                   @cancel="replying = false"/>

  </div>
</template>

<script>
import CommentInput from "./comment-input";
import {configurable, creator} from "@/components/comments/_shared";
import UserHeader from "@/components/user-header";

export default {
  name: "comment-body",
  mixins: [creator, configurable],
  components: {UserHeader, CommentInput},
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
