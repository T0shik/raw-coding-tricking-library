<template>
  <v-dialog :value="active" persistent>
    <template v-slot:activator="{on}">
      <v-menu offset-y>
        <template v-slot:activator="{ on, attrs }">
          <v-btn depressed v-bind="attrs" v-on="on">
            Create
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

    <div class="d-flex justify-center my-4">
      <v-btn @click="reset">
        Close
      </v-btn>
    </div>
  </v-dialog>
</template>

<script>
  import {mapState, mapMutations} from 'vuex';
  import TrickSteps from "./trick-steps";
  import SubmissionSteps from "./submission-steps";

  export default {
    name: "content-creation-dialog",
    components: {SubmissionSteps, TrickSteps},
    computed: {
      ...mapState('video-upload', ['active', 'component']),
      menuItems() {
        return [
          {component: TrickSteps, title: "Trick"},
          {component: SubmissionSteps, title: "Submission"},
        ]
      }
    },
    methods: mapMutations('video-upload', ['reset', 'activate']),
  }
</script>

<style scoped>

</style>
