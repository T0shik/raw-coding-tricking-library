<template>
  <item-content-layout v-if="profile">
    <template v-slot:content>
      <submission-feed :content-endpoint="`/api/users/${profile.id}/submissions`"/>
    </template>
    <template v-slot:item>
      <user-header :username="profile.username" :image-url="profile.image" :link="false"/>
      <v-divider class="my-2"/>
      <div>
        <h6 class="text-h6 mb-2">Completed Tricks ({{ completedTricks.length }} / {{ lists.tricks.length }})</h6>
        <v-chip class="mb-1 mr-1" small
                v-for="{submission, trick} in completedTricks"
                @click="goToSubmission(trick.slug, submission.id)"
                :key="`profile-trick-chip-${submission.id}`">
          {{ trick.name }}
        </v-chip>
      </div>
    </template>
  </item-content-layout>
</template>

<script>
import ItemContentLayout from "@/components/item-content-layout";
import Submission from "@/components/submission";
import SubmissionFeed from "@/components/submission-feed";
import UserHeader from "@/components/user-header";
import profile from "@/components/profile";

export default {
  components: {UserHeader, SubmissionFeed, Submission, ItemContentLayout},
  mixins: [profile],
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
