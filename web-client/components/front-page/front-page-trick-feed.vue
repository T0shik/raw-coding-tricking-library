<template>
  <v-row>
    <v-col class="d-flex justify-center align-start" v-for="trick in content" :key="`trick-feed-${trick.id}`">
      <v-card width="320" @click="() => $router.push(`/trick/${trick.slug}`)" :ripple="false">
        <v-card-title>{{ trick.name }}</v-card-title>
        <submission v-if="trick.submission" :submission="trick.submission" slim/>
        <v-card-text>{{ trick.description }}</v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script>
import {feed} from "@/components/feed";
import {mapState} from "vuex";
import Submission from "@/components/submission";

export default {
  name: "front-page-trick-feed",
  components: {Submission},
  mixins: [feed('')],
  fetch() {
    return this.loadContent()
  },
  methods: {
    loadContent() {
      const maxRange = this.lists.tricks.length
      let to = this.cursor + this.limit
      if (to >= maxRange) {
        to = maxRange
      }
      const tricks = this.lists.tricks.slice(this.cursor, to)
      this.cursor += this.limit
      return Promise.all(tricks.map(trick => this.$axios
        .$get(`/api/tricks/${trick.slug}/best-submission`)
        .then(submission => this.content.push({
          ...trick,
          submission
        }))))
    }
  },
  computed: mapState('tricks', ['lists'])
}
</script>

<style scoped>
.v-card--link:focus:before {
  opacity: 0;
}
</style>
