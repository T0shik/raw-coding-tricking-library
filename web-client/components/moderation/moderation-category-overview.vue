<template>
  <v-list>
    <v-list-item @click="$router.push(`/category/${category.slug}`)"
                 :key="`moderation-category-${category.id}`"
                 v-for="category in lists.categories">
      <v-list-item-content>
        <v-list-item-title>{{ category.name }}</v-list-item-title>
        <v-list-item-subtitle>{{ category.description }}</v-list-item-subtitle>
      </v-list-item-content>
      <v-list-item-content>
        <v-list-item-title>Last Updated</v-list-item-title>
        <v-list-item-subtitle>{{ category.updated }}</v-list-item-subtitle>
      </v-list-item-content>
      <v-list-item-action>
        <v-btn icon @click.stop="edit(category)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>
      </v-list-item-action>
      <v-list-item-action>
        <v-btn icon @click.stop="">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </v-list-item-action>
    </v-list-item>
  </v-list>
</template>

<script>
import {mapMutations, mapState} from "vuex";
import CategoryForm from '@/components/content-creation/category-form'

export default {
  name: "moderation-category-overview",
  methods: {
    ...mapMutations('content-creation', ['activate']),
    edit(category) {
      this.activate({
        component: CategoryForm,
        editPayload: category,
        setup: null
      })
    }
  },
  computed: mapState('library', ['lists'])
}
</script>

<style scoped>

</style>
