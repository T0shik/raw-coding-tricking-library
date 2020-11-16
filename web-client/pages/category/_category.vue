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
    ...mapState('tricks', ['dictionary']),
    tricks() {
      const categorySlug = this.$route.params.category;
      return this.dictionary.categories[categorySlug]
        .tricks
        .map(x => this.dictionary.tricks[x])
    },
    category() {
      return this.dictionary.categories[this.$route.params.category]
    }
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
