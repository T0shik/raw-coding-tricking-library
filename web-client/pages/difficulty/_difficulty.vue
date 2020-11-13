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
import TrickList from "../../components/trick-list";
import ItemContentLayout from "../../components/item-content-layout";

export default {
  components: {ItemContentLayout, TrickList},
  computed: {
    ...mapState('tricks', ['lists', 'dictionary']),
    tricks() {
      const difficultyId = this.$route.params.difficulty;
      return this.lists.tricks.filter(x => x.difficulty === difficultyId)
    },
    difficulty() {
      const difficultyId = this.$route.params.difficulty;
      return this.dictionary.difficulties[difficultyId]
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
