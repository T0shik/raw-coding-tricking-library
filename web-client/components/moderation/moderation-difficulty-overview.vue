<template>
  <div>
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
          <v-btn icon @click.stop="selectedDifficulty = difficulty">
            <v-icon>mdi-swap-horizontal</v-icon>
          </v-btn>
        </v-list-item-action>
      </v-list-item>
    </v-list>
    <v-dialog :value="selectedDifficulty" width="300" persistent>
      <v-card v-if="selectedDifficulty">
        <v-card-title>Migrate {{ selectedDifficulty.name }}?</v-card-title>
        <v-card-text>
          <v-select
            :items="lists.difficulties.filter(x => x.id !== selectedDifficulty.id).map(x => ({value: x.id, text: x.name}))"
            v-model="target"
            label="Difficulty"/>
        </v-card-text>
        <v-card-actions>
          <v-btn color="primary" @click="selectedDifficulty = null">no</v-btn>
          <v-spacer/>
          <v-btn :disabled="target === 0" @click="migrate">yes</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import {mapMutations, mapState} from "vuex";
import DifficultyForm from "@/components/content-creation/difficulty-form";
import {EVENTS} from "@/data/events";

export default {
  name: "moderation-difficulty-overview",
  data: () => ({
    selectedDifficulty: null,
    target: 0
  }),
  methods: {
    ...mapMutations('content-creation', ['activate']),
    edit(difficulty) {
      this.activate({
        component: DifficultyForm,
        editPayload: difficulty,
        setup: null
      })
    },
    migrate() {
      return this.$axios.put(`/api/difficulties/${this.selectedDifficulty.id}/${this.target}`, null)
        .then(() => this.$nuxt.$emit(EVENTS.CONTENT_UPDATED))
        .finally(() => {
          this.selectedDifficulty = null
          this.target = 0
        })
    }
  },
  computed: mapState('library', ['lists'])
}
</script>

<style scoped>

</style>
