<template>
  <div>
    <comment-input label="comment"
                   :parent-id="parentId"
                   :parent-type="parentType"
                   @comment-created="(c) => content.unshift(c)"/>

    <v-divider class="my-5"/>

    <comment v-for="comment in content" :comment="comment" :key="comment.id"/>
    <div class="d-flex justify-center" v-if="!finished">
      <v-btn outlined small @click="loadContent">load more</v-btn>
    </div>
  </div>
</template>

<script>
import CommentInput from "./comment-input";
import Comment from "./comment";
import {configurable} from "@/components/comments/_shared";
import {feed} from "@/components/feed";

export default {
  name: "comment-section",
  components: {Comment, CommentInput},
  mixins: [configurable, feed('first')],
  created() {
    this.loadContent()
  },
  methods: {
    getContentUrl() {
      return `/api/comments/${this.parentId}/${this.parentType}${this.query}`
    }
  }
}
</script>

<style scoped>

</style>
