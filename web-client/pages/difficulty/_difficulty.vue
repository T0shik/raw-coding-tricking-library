<template>
  <div class="d-flex mt-3 justify-center align-start">
    <trick-list :tricks="tricks" class="mx-2" />

    <v-sheet class="pa-3 mx-2 sticky" v-if="difficulty">
      <div class="text-h6">{{ difficulty.name }}</div>
      <v-divider class="my-1"></v-divider>
      <div class="text-body-2">{{ difficulty.description }}</div>
    </v-sheet>
  </div>
</template>

<script>
  import {mapGetters} from 'vuex'
  import TrickList from "../../components/trick-list";

  export default {
    components: {TrickList},
    data: () => ({
      difficulty: null,
      tricks: [],
    }),
    computed: mapGetters('tricks', ['difficultyById']),
    async fetch() {
      const difficultyId = this.$route.params.difficulty;
      this.difficulty = this.difficultyById(difficultyId)
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
