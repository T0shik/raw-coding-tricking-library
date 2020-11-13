<template>
  <div v-scroll="onScroll">
    <v-tabs v-model="tab" grow>
      <v-tab>Latest</v-tab>
      <v-tab>Top</v-tab>
    </v-tabs>
    <submission class="my-3" :submission="s" v-for="s in content" :key="`submission-${s.id}`"/>
  </div>
</template>

<script>
import Submission from "@/components/submission";
import {feed} from "@/components/feed";

export default {
  name: "submission-feed",
  components: {Submission},
  mixins: [feed('latest')],
  data: () => ({
    tab: 0
  }),
  async fetch() {
    await this.loadContent()
    if (this.$route.query.submission) {
      const submission = await this.$axios.$get(`/api/submissions/${this.$route.query.submission}`)

      const existingIndex = this.content.map(x => x.id).indexOf(submission.id)
      if (existingIndex > -1) {
        this.content.splice(existingIndex, 1)
      }

      this.content.unshift(submission)
    }
  },
  watch: {
    'tab': function (n) {
      this.order = n === 0 ? 'latest' :
        n === 1 ? 'top' :
          'latest'
    },
  }
}
</script>

<style scoped>

</style>
