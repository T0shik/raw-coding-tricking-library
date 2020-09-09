<template>
  <v-card>
    <v-card-title>
      <v-avatar>
        <v-icon>mdi-account</v-icon>
      </v-avatar>
      Test user
    </v-card-title>

    <v-card-text>
      <div v-if="submissions">
        <v-card class="mb-3" v-for="s in submissions" :key="`${s.id}`">
          <video-player :video="s.video" :key="`v-${s.id}`"/>
          <v-card-text>{{ s.description }}</v-card-text>
        </v-card>
      </div>
    </v-card-text>

  </v-card>
</template>

<script>
import VideoPlayer from "@/components/video-player";

export default {
  components: {VideoPlayer},
  data: () => ({
    submissions: []
  }),
  mounted() {
    return this.$store.dispatch('auth/_watchUserLoaded', async () => {
      const profile = this.$store.state.auth.profile
      console.log("mounted profile", profile)
      this.submissions = await this.$axios.$get(`/api/users/${profile.id}/submissions`)
    })
  }
}
</script>

<style scoped>

</style>
