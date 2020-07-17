<template>
  <div class="d-flex mt-3 justify-center align-start">
    <trick-list :tricks="tricks" class="mx-2" />

    <v-sheet class="pa-3 mx-2 sticky" v-if="category">
      <div class="text-h6">{{ category.name }}</div>
      <v-divider class="my-1"></v-divider>
      <div class="text-body-2">{{ category.description }}</div>
    </v-sheet>
  </div>
</template>

<script>
  import {mapGetters} from 'vuex'
  import TrickList from "../../components/trick-list";

  export default {
    components: {TrickList},
    data: () => ({
      category: null,
      tricks: [],
    }),
    computed: mapGetters('tricks', ['categoryById']),
    async fetch() {
      const categoryId = this.$route.params.category;
      this.category = this.categoryById(categoryId)
      this.tricks = await this.$axios.$get(`/api/categories/${categoryId}/tricks`)
    },
    head() {
      if (!this.category) return {}

      return {
        title: this.category.name,
        meta: [
          {hid: 'description', name: 'description', content: this.category.description}
        ]
      }
    }
  }
</script>

<style scoped>

</style>
