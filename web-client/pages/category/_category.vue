<template>
  <div>
    <div>
      <div class="text-h4">{{ category.name }}</div>
      <div class="text-body-1">{{ category.description }}</div>
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
      const categoryId = this.$route.params.category;
      return this.lists.tricks.filter(x => x.categories.indexOf(categoryId) > -1)
    },
    category() {
      const categoryId = this.$route.params.category;
      return this.dictionary.categories[categoryId]
    }
  },
  async fetch() {
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
