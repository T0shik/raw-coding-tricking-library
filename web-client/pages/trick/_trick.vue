<template>
  <item-content-layout>
    <template v-slot:content>
      <submission :submission="s" v-for="s in submissions" :key="`submission-${s.id}`"/>
    </template>
    <template v-slot:item="{close}">
      <trick-info-card :trick="trick" :close="close" />
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

export default {
  components: {TrickInfoCard, Submission, ItemContentLayout},
  data: () => ({
    trick: null,
  }),
  computed: {
    ...mapState('submissions', ['submissions']),
    ...mapState('tricks', ['dictionary']),
  },
  async fetch() {
    const trickSlug = this.$route.params.trick;
    this.trick = this.dictionary.tricks[trickSlug]
    await this.$store.dispatch("submissions/fetchSubmissionsForTrick", {trickId: trickSlug}, {root: true})
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
