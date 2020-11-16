<template>
  <v-dialog :value="active" persistent width="700">
    <template v-slot:activator="{on}">
      <v-menu offset-y>
        <template v-slot:activator="{ on, attrs }">
          <v-btn class="d-none d-md-flex" depressed v-bind="attrs" v-on="on">
            Create
          </v-btn>
          <v-btn class="d-flex d-md-none" icon v-bind="attrs" v-on="on">
            <v-icon>mdi-plus-box</v-icon>
          </v-btn>
        </template>
        <v-list>
          <v-list-item v-for="(item, i) in menuItems" :key="`ccd-menu-${i}`"
                       @click="activate({component:item.component})">

            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </template>

    <div v-if="component">
      <component :is="component"></component>
    </div>
  </v-dialog>
</template>

<script>
import {mapState, mapMutations, mapGetters} from 'vuex';
import TrickSteps from "./trick-steps";
import SubmissionSteps from "./submission-steps";
import DifficultyForm from "./difficulty-form";
import CategoryForm from "./category-form";

export default {
  name: "content-creation-dialog",
  components: {CategoryForm, DifficultyForm, SubmissionSteps, TrickSteps},
  computed: {
    ...mapState('content-update', ['active', 'component']),
    ...mapGetters('auth', ['moderator']),
    menuItems() {
      return [
        {component: TrickSteps, title: "Trick", display: true},
        {component: SubmissionSteps, title: "Submission", display: true},
        {component: DifficultyForm, title: "Difficulty", display: this.moderator},
        {component: CategoryForm, title: "Category", display: this.moderator},
      ].filter(x => x.display)
    }
  },
  methods: mapMutations('content-update', ['activate']),
}
</script>

<style scoped>

</style>
