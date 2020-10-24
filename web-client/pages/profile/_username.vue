<template>
  <item-content-layout v-if="profile">
    <template v-slot:content>
      <submission-feed :content-endpoint="`/api/users/${profile.id}/submissions`"/>
    </template>
    <template v-slot:item>
      <user-header :username="profile.username" :image-url="profile.image" :link="false"/>
    </template>
  </item-content-layout>
</template>

<script>
import ItemContentLayout from "@/components/item-content-layout";
import Submission from "@/components/submission";
import SubmissionFeed from "@/components/submission-feed";
import UserHeader from "@/components/user-header";

export default {
  components: {UserHeader, SubmissionFeed, Submission, ItemContentLayout},
  data: () => ({
    profile: null
  }),
  async fetch() {
    const {username} = this.$route.params
    this.profile = await this.$axios.$get(`/api/users/${username}`)
  }
}
</script>

<style scoped>

</style>
