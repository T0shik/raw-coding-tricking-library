import {mapState} from "vuex";

export default {
  methods: {
    goToSubmission(trickSlug, submissionId) {
      this.$router.push(`/trick/${trickSlug}?submission=${submissionId}`)
    }
  },
  computed: {
    ...mapState('tricks', ['lists','dictionary']),
    completedTricks() {
      const submissions = this.profile.submissions.filter((v, i, a) => a.map(x => x.trickId).indexOf(v.trickId) === i);
      return submissions.map(submission => ({
        submission,
        trick: this.dictionary.tricks[submission.trickId]
      }))
    }
  }
}
