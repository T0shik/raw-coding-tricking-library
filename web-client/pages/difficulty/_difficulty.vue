<template>
  <item-content-layout>
    <template v-slot:content>
      <trick-list :tricks="tricks"/>
    </template>
    <template v-slot:item>
      <div v-if="difficulty">
        <div class="text-h6">{{ difficulty.name }}</div>
        <v-divider class="my-1"></v-divider>
        <div class="text-body-2">{{ difficulty.description }}</div>
      </div>
    </template>
  </item-content-layout>
</template>

<script>
import {mapState} from 'vuex'
  import TrickList from "../../components/trick-list";
  import ItemContentLayout from "../../components/item-content-layout";

  export default {
    components: {ItemContentLayout, TrickList},
    data: () => ({
      difficulty: null,
      tricks: [],
    }),
    computed: mapState('tricks', ['dictionary']),
    async fetch() {
      const difficultyId = this.$route.params.difficulty;
      this.difficulty = this.dictionary.difficulties[difficultyId]
      this.tricks = await this.$axios.$get(`/api/difficulties/${difficultyId}/tricks`)
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
