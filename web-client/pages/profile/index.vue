<template>
  <item-content-layout>
    <template v-slot:content>
      <submission :submission="s" v-for="s in submissions" :key="`submission-${s.id}`"/>
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
              <img v-else-if="profile.image" :src="profile.image" alt="profile image" />
              <v-icon v-else>mdi-account</v-icon>
            </v-avatar>
          </v-hover>
          {{ profile.username }}
        </div>
      </div>
    </template>
  </item-content-layout>
</template>

<script>
import ItemContentLayout from "@/components/item-content-layout";
import {mapMutations, mapState} from "vuex";
import Submission from "@/components/submission";

export default {
  components: {Submission, ItemContentLayout},
  data: () => ({
    submissions: [],
    uploadingImage: false
  }),
  mounted() {
    return this.$store.dispatch('auth/_watchUserLoaded', async () => {
      const profile = this.$store.state.auth.profile
      console.log("mounted profile", profile)
      this.submissions = await this.$axios.$get(`/api/users/${profile.id}/submissions`)
    })
  },
  methods: {
    changeProfileImage(e) {
      if (this.uploadingImage) return;
      this.uploadingImage = true
      const fileInput = e.target;
      const formData = new FormData();
      formData.append('image', fileInput.files[0])

      console.log(fileInput)

      return this.$axios.$put('/api/users/me/image', formData)
        .then(profile => {
          this.saveProfile({profile})
          fileInput.value = ""
          this.uploadingImage = false
        })
    },
    ...mapMutations('auth', ['saveProfile'])
  },
  computed: mapState('auth', ['profile'])
}
</script>

<style scoped>

</style>
