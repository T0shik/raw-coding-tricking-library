<template>
  <div>
    <div>
      <div class="text-h4">{{ difficulty.name }}</div>
      <div class="text-body-1">{{ difficulty.description }}</div>
    </div>
    <v-divider class="my-3"/>
    <trick-list :tricks="tricks"/>
  </div>
</template>

<script>
import {mapState} from 'vuex'
import TrickList from "@/components/trick-list";
import ItemContentLayout from "@/components/item-content-layout";

export default {
  components: {ItemContentLayout, TrickList},
  computed: {
    ...mapState('library', ['lists', 'dictionary']),
    tricks() {
      const difficultySlug = this.$route.params.difficulty;
      return this.dictionary.difficulties[difficultySlug]
        .tricks
        .map(x => this.dictionary.tricks[x])
    },
    difficulty() {
      return this.dictionary.difficulties[this.$route.params.difficulty]
    }
  },
  head() {
    if (!this.difficulty) return {}

    return {
      title: this.difficulty.name,
      meta: [
        {hid: 'description', name: 'description', content: this.difficulty.description}
      ]
    }
  }
}
</script>

<style scoped>

</style>
