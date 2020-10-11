<template>
  <div v-scroll="onScroll">
    <v-tabs v-model="tab" grow>
      <v-tab>Latest</v-tab>
      <v-tab>Top</v-tab>
    </v-tabs>
    <submission :submission="s" v-for="s in submissions" :key="`submission-${s.id}`"/>
  </div>
</template>

<script>
import Submission from "@/components/submission";

export default {
  name: "submission-feed",
  components: {Submission},
  props: {
    loadSubmissions: {
      type: Function,
      required: true
    }
  },
  data: () => ({
    submissions: [],
    cursor: 0,
    tab: 0,
    finished: false,
    loading: false
  }),
  watch: {
    'tab': function () {
      this.submissions = []
      this.cursor = 0
      this.finished = false
      this.handleSubmissions()
    }
  },
  created() {
    this.handleSubmissions()
  },
  methods: {
    onScroll() {
      if (this.finished || this.loading) return;
      const loadMore = document.body.offsetHeight - (window.pageYOffset + window.innerHeight) < 500

      if (loadMore) {
        this.handleSubmissions()
      }
    },
    handleSubmissions() {
      this.loading = true
      return this.loadSubmissions(this.query)
        .then(submissions => {
          console.log(submissions)
          if (submissions.length === 0) {
            this.finished = true
          } else {
            submissions.forEach(x => this.submissions.push(x))
            this.cursor += 10;
          }
        })
        .finally(() => this.loading = false)
    }
  },
  computed: {
    order() {
      return this.tab === 0 ? 'latest' :
        this.tab === 1 ? 'top' :
          'latest'
    },
    query() {
      return `?order=${this.order}&cursor=${this.cursor}`
    }
  }
}
</script>

<style scoped>

</style>
