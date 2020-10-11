<template>
  <item-content-layout>
    <template v-slot:content>
      <submission-feed :load-submissions="loadSubmissions"/>
    </template>
    <template v-slot:item="{close}">
      <trick-info-card :trick="trick" :close="close"/>
    </template>
  </item-content-layout>
</template>

<script>
// todo: clean up submission id's ^^^
import {mapState, mapMutations} from 'vuex';
import TrickSteps from "@/components/content-creation/trick-steps";
import ItemContentLayout from "../../components/item-content-layout";
import Submission from "@/components/submission";
import TrickInfoCard from "@/components/trick-info-card";
import SubmissionFeed from "@/components/submission-feed";

export default {
  components: {SubmissionFeed, TrickInfoCard, Submission, ItemContentLayout},
  computed: {
    ...mapState('tricks', ['dictionary']),
    trick() {
      return this.dictionary.tricks[this.$route.params.trick]
    }
  },
  methods: {
    loadSubmissions(query) {
      return this.$axios.$get(`/api/tricks/${this.trick.slug}/submissions${query}`)
    }
  },
  head() {
    if (!this.trick) return {}

    return {
      title: this.trick.name,
      meta: [
        {hid: 'description', name: 'description', content: this.trick.description}
      ]
    }
  }
}
</script>

<style scoped></style>
