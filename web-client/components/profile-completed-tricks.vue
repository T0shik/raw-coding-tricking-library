<template>
  <div>
    <h6 class="text-h6 mb-2">Completed Tricks ({{ completedTricks.length }} / {{ lists.tricks.length }})</h6>
    <v-chip class="mb-1 mr-1" small
            v-for="{submission, trick} in completedTricks"
            @click="goToSubmission(trick.slug, submission.id)"
            :key="`profile-trick-chip-${submission.id}`">
      {{ trick.name }}
    </v-chip>
  </div>
</template>

<script>
import {mapState} from "vuex";

export default {
  name: "profile-completed-tricks",
  props: {
    profileSubmissions: {
      required: true,
      type: Array
    }
  },
  methods: {
    goToSubmission(trickSlug, submissionId) {
      this.$router.push(`/trick/${trickSlug}?submission=${submissionId}`)
    }
  },
  computed: {
    submissions() {
      return [...this.profileSubmissions].sort((a, b) => b.score - a.score)
    },
    ...mapState('library', ['lists', 'dictionary']),
    completedTricks() {
      const submissions = this.submissions.filter((v, i, a) => a.map(x => x.trickId).indexOf(v.trickId) === i);
      return submissions.map(submission => ({
        submission,
        trick: this.dictionary.tricks[submission.trickId]
      }))
    }
  }
}
</script>

<style scoped>

</style>
