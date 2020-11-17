<template>
  <div>
    <v-row justify="space-around">
      <v-col lg="3" class="d-flex justify-center align-start" v-for="difficulty in content"
             :key="`difficulty-feed-${difficulty.id}`">
        <v-card width="320" @click="() => $router.push(`/difficulty/${difficulty.slug}`)" :ripple="false">
          <v-card-title>{{ difficulty.name }}</v-card-title>
          <v-divider />
          <submission v-if="difficulty.submission"
                      :submission="difficulty.submission"
                      slim
                      elevation="0"/>
          <v-card-text>{{ difficulty.description }}</v-card-text>
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
  name: "front-page-difficulty-feed",
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
      const maxRange = this.lists.difficulties.length
      let to = this.cursor + this.limit
      if (to >= maxRange) {
        to = maxRange
        this.finished = true
      }
      const difficulties = this.lists.difficulties.slice(this.cursor, to)
      this.cursor += this.limit

      const submissionRequests = difficulties.map(difficulty => {
        if (difficulty.tricks.length > 0) {
          const byTricks = difficulty.tricks
            .map(x => this.dictionary.tricks[x].slug)
            .reduce((a, b) => `${a};${b}`, "")

          return this.$axios
            .$get(`/api/submissions/best-submission?byTricks=${byTricks}`)
            .then(submission => this.content.push({
              ...difficulty,
              submission
            }))
        } else {
          this.content.push(difficulty)
        }
      })
      return Promise.all(submissionRequests)
    }
  },
  computed: mapState('library', ['lists', 'dictionary'])
}
</script>

<style scoped>
.v-card--link:focus:before {
  opacity: 0;
}
</style>
