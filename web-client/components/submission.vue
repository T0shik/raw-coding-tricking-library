<template>
  <v-card class="my-3">
    <user-header :username="submission.user.username" :image-url="submission.user.image" :append="submission.created"/>
    <v-card-text>{{ submission.description }}</v-card-text>
    <video-player :video="submission.video" :thumb="submission.thumb"/>
    <v-card-actions>
      <span>{{ submission.upVotes }}</span>
      <v-btn icon>
        <v-icon>mdi-thumb-up</v-icon>
      </v-btn>
      <v-spacer/>
      <v-btn icon @click="showComments = !showComments">
        <v-icon>mdi-comment</v-icon>
      </v-btn>
    </v-card-actions>
    <if-auth v-if="showComments" class="px-3 pb-2">
      <template v-slot:allowed>
        <comment-section :parent-id="submission.id" :parent-type="submissionParentType"/>
      </template>
      <template v-slot:forbidden="{login}">
        <div class="d-flex justify-center">
          <v-btn outlined @click="login">sign in to comment</v-btn>
        </div>
      </template>
    </if-auth>
  </v-card>
</template>

<script>
import UserHeader from "@/components/user-header";
import VideoPlayer from "@/components/video-player";
import {COMMENT_PARENT_TYPE} from "@/components/comments/_shared";
import CommentSection from "@/components/comments/comment-section";
import IfAuth from "@/components/auth/if-auth";

export default {
  name: "submission",
  components: {IfAuth, CommentSection, VideoPlayer, UserHeader},
  props: {
    submission: {
      type: Object,
      required: true,
    }
  },
  data: () => ({
    showComments: false
  }),
  computed: {
    submissionParentType() {
      return COMMENT_PARENT_TYPE.SUBMISSION
    }
  }
}
</script>

<style scoped>

</style>
