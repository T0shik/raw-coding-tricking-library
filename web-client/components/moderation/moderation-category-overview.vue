<template>
  <div>
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
          <v-btn icon @click.stop="selectedCategory = category">
            <v-icon>mdi-delete</v-icon>
          </v-btn>
        </v-list-item-action>
      </v-list-item>
    </v-list>
    <v-dialog :value="selectedCategory" width="300" persistent>
      <v-card v-if="selectedCategory">
        <v-card-title>Delete {{ selectedCategory.name }}?</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="selectedCategory = null">no</v-btn>
          <v-spacer/>
          <v-btn @click="confirmDelete">yes</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import {mapMutations, mapState} from "vuex";
import CategoryForm from '@/components/content-creation/category-form'
import {EVENTS} from "@/data/events";

export default {
  name: "moderation-category-overview",
  data: () => ({
    selectedCategory: null
  }),
  methods: {
    ...mapMutations('content-creation', ['activate']),
    edit(category) {
      this.activate({
        component: CategoryForm,
        editPayload: category,
        setup: null
      })
    },
    confirmDelete() {
      return this.$axios.$delete(`/api/categories/${this.selectedCategory.id}`)
        .then(() => this.$nuxt.$emit(EVENTS.CONTENT_UPDATED))
        .finally(() => this.selectedCategory = null)
    }
  },
  computed: mapState('library', ['lists'])
}
</script>

<style scoped>

</style>
