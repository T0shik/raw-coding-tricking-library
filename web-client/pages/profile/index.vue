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
        <div class="mt-2">
          Role: <strong>{{ profile.role }}</strong>
        </div>
        <v-divider class="my-2"/>
        <profile-completed-tricks :profile-submissions="profile.submissions"/>
        <div v-if="moderationItems.length > 0">
          <v-divider class="my-2"/>
          <h5 class="text-h5">Change Requests</h5>
          <v-list>
            <v-list-item v-for="modItem in moderationItems"
                         :key="`profile-modItem-${modItem.id}`"
                         :to="`/moderation/${modItem.id}`">
              <v-list-item-content>
                <v-list-item-title v-if="modItem.currentObject">{{ modItem.currentObject.name }}</v-list-item-title>
                <v-list-item-title v-else-if="modItem.targetObject">{{ modItem.targetObject.name }}</v-list-item-title>
                <v-list-item-subtitle>
                  <span>Type: {{ modItem.type }},</span>
                  <span v-if="modItem.currentObject">Version: {{ modItem.currentObject.version }}</span>
                  <span v-else-if="modItem.targetObject">Version: {{ modItem.targetObject.version }}</span>
                </v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
          </v-list>
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
import ProfileCompletedTricks from "@/components/profile-completed-tricks";

export default {
  components: {ProfileCompletedTricks, SubmissionFeed, Submission, ItemContentLayout},
  data: () => ({
    uploadingImage: false,
    moderationItems: []
  }),
  fetch() {
    return this.$axios.$get(`/api/moderation-items?user=1`)
      .then(moderationItems => this.moderationItems = moderationItems)
  },
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
