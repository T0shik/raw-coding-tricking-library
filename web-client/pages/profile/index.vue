<template>
  <item-content-layout>
    <template v-slot:content>
      <submission-feed :content-endpoint="`/api/users/${$store.state.auth.profile.id}/submissions`"/>
    </template>
    <template v-slot:item>
      <div v-if="profile">
        <div>
          <input class="d-none" type="file" accept="image/*" ref="profileImageInput" @change="changeProfileImage"/>
          <v-hover v-slot:default="{hover}">
            <v-avatar>
              <v-btn icon v-if="hover" :disabled="uploadingImage" @click="$refs.profileImageInput.click()">
                <v-icon>mdi-account-edit</v-icon>
              </v-btn>
              <img v-else-if="profile.image" :src="profile.image" alt="profile image"/>
              <v-icon v-else>mdi-account</v-icon>
            </v-avatar>
          </v-hover>
          {{ profile.username }}
        </div>
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
      </div>
    </template>
  </item-content-layout>
</template>

<script>
import ItemContentLayout from "@/components/item-content-layout";
import {mapMutations, mapState} from "vuex";
import Submission from "@/components/submission";
import SubmissionFeed from "@/components/submission-feed";
import profile from "@/components/profile";

export default {
  components: {SubmissionFeed, Submission, ItemContentLayout},
  mixins: [profile],
  data: () => ({
    uploadingImage: false
  }),
  methods: {
    changeProfileImage(e) {
      if (this.uploadingImage) return;
      this.uploadingImage = true
      const fileInput = e.target;
      const formData = new FormData();
      formData.append('image', fileInput.files[0])

      return this.$axios.$put('/api/users/me/image', formData)
        .then(profile => {
          this.saveProfile({profile})
          fileInput.value = ""
          this.uploadingImage = false
        })
    },
    ...mapMutations('auth', ['saveProfile'])
  },
  computed: mapState('auth', ['profile']),
}
</script>

<style scoped>

</style>
