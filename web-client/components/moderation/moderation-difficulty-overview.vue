<template>
  <v-list>
    <v-list-item @click="$router.push(`/difficulty/${difficulty.slug}`)"
                 :key="`moderation-difficulty-${difficulty.id}`"
                 v-for="difficulty in lists.difficulties">
      <v-list-item-content>
        <v-list-item-title>{{ difficulty.name }}</v-list-item-title>
        <v-list-item-subtitle>{{ difficulty.description }}</v-list-item-subtitle>
      </v-list-item-content>
      <v-list-item-content>
        <v-list-item-title>Last Updated</v-list-item-title>
        <v-list-item-subtitle>{{ difficulty.updated }}</v-list-item-subtitle>
      </v-list-item-content>
      <v-list-item-action>
        <v-btn icon @click.stop="edit(difficulty)">
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
import DifficultyForm from "@/components/content-creation/difficulty-form";

export default {
  name: "moderation-difficulty-overview",
  methods: {
    ...mapMutations('content-creation', ['activate']),
    edit(difficulty) {
      this.activate({
        component: DifficultyForm,
        editPayload: difficulty,
        setup: null
      })
    }
  },
  computed: mapState('library', ['lists'])
}
</script>

<style scoped>

</style>
