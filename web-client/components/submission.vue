<template>
  <v-card class="mb-3">
    <user-header :username="submission.user.username" :image-url="submission.user.image" />
    <video-player :video="submission.video" :thumb="submission.thumb"/>
    <v-card-text>{{ submission.description }}</v-card-text>
  </v-card>
</template>

<script>
import UserHeader from "@/components/user-header";
import VideoPlayer from "@/components/video-player";
export default {
  name: "submission",
  components: {VideoPlayer, UserHeader},
  props: {
    submission: {
      type: Object,
      required: true,
    }
  }
}
</script>

<style scoped>

</style>
