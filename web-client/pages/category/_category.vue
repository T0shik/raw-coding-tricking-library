<template>
  <div class="d-flex mt-3 justify-center align-start">
    <div class="mx-2">
      <v-text-field label="Search" placeholder="e.g. cork/flip/kick" v-model="filter"
                    prepend-inner-icon="mdi-magnify" outlined></v-text-field>
      <div v-for="t in filteredTricks">
        {{t.name}} - {{t.description}}
      </div>
    </div>

    <v-sheet class="pa-3 mx-2 sticky" v-if="category">
      <div class="text-h6">{{ category.name }}</div>
      <v-divider class="my-1"></v-divider>
      <div class="text-body-2">{{ category.description }}</div>
    </v-sheet>
  </div>
</template>

<script>
  import {mapGetters} from 'vuex'

  export default {
    data: () => ({
      category: null,
      tricks: [],
      filter: "",
    }),
    computed: {
      ...mapGetters('tricks', ['categoryById']),
      filteredTricks() {
        if (!this.filter) return this.tricks

        const normilze = this.filter.trim().toLowerCase();
        return this.tricks.filter(t => t.name.toLowerCase().includes(normilze)
          || t.description.toLowerCase().includes(normilze))

      }
    },
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
