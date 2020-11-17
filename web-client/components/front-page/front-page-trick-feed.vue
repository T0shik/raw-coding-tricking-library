<template>
  <div>
    <v-row justify="space-around">
      <v-col lg="3" class="d-flex justify-center align-start" v-for="trick in content" :key="`trick-feed-${trick.id}`">
        <v-card width="320" @click="() => $router.push(`/trick/${trick.slug}`)" :ripple="false">
          <v-card-title>{{ trick.name }}</v-card-title>
          <v-divider />
          <submission v-if="trick.submission"
                      :submission="trick.submission"
                      slim
                      elevation="0"/>
          <v-card-text>{{ trick.description }}</v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <div v-if="!finished" class="d-flex justify-center">
      <v-btn @click="loadContent">Load More</v-btn>
    </div>
  </div>
</template>

<script>
import {feed} from "@/components/feed";
import {mapState} from "vuex";
import Submission from "@/components/submission";

export default {
  name: "front-page-trick-feed",
  components: {Submission},
  mixins: [feed('')],
  data: () => ({
    limit: 8
  }),
  fetch() {
    return this.loadContent()
  },
  methods: {
    loadContent() {
      const maxRange = this.lists.tricks.length
      let to = this.cursor + this.limit
      if (to >= maxRange) {
        to = maxRange
        this.finished = true
      }
      const tricks = this.lists.tricks.slice(this.cursor, to)
      this.cursor += this.limit
      const submissionRequests = tricks.map(trick => this.$axios
        .$get(`/api/submissions/best-submission?byTricks=${trick.slug}`)
        .then(submission => this.content.push({
          ...trick,
          submission
        })));
      return Promise.all(submissionRequests)
    }
  },
  computed: mapState('library', ['lists'])
}
</script>

<style scoped>
.v-card--link:focus:before {
  opacity: 0;
}
</style>
