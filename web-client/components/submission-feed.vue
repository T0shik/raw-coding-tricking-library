<template>
  <div v-scroll="onScroll">
    <v-tabs v-model="tab" grow>
      <v-tab>Latest</v-tab>
      <v-tab>Top</v-tab>
    </v-tabs>
    <submission :submission="s" v-for="s in content" :key="`submission-${s.id}`"/>
  </div>
</template>

<script>
import Submission from "@/components/submission";
import {feed} from "@/components/feed";

export default {
  name: "submission-feed",
  components: {Submission},
  mixins: [feed('', true)],
  data: () => ({
    tab: 0
  }),
  watch: {
    'tab': {
      handler: function (n) {
        this.order = n === 0 ? 'latest' :
          n === 1 ? 'top' :
            'latest'
      },
      immediate: true
    }
  }
}
</script>

<style scoped>

</style>
