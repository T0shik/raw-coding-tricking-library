<template>
  <item-content-layout>
    <template v-slot:content>
      <submission :submission="s" v-for="s in submissions" :key="`submission-${s.id}`"/>
    </template>
    <template v-slot:item="{close}">
      <div class="text-h5">
        <span>{{ trick.name }}</span>
        <v-chip class="mb-1 ml-2" small :to="`/difficulty/${difficulty.slug}`">
          {{ difficulty.name }}
        </v-chip>
      </div>
      <v-divider class="my-1"></v-divider>
      <div class="text-body-2">{{ trick.description }}</div>
      <v-divider class="my-1"></v-divider>
      <div v-for="rd in relatedData" v-if="rd.data.length > 0">
        <div class="text-subtitle-1">{{ rd.title }}</div>
        <v-chip-group>
          <v-chip v-for="d in rd.data" :key="rd.idFactory(d)" small :to="rd.routeFactory(d)">
            {{ d.name }}
          </v-chip>
        </v-chip-group>
      </div>
      <v-divider class="mb-2"></v-divider>
      <div>
        <v-btn @click="edit(); close();" outlined small>edit</v-btn>
      </div>
    </template>
  </item-content-layout>
</template>

<script>
// todo: clean up submission id's ^^^
import {mapState, mapMutations} from 'vuex';
import TrickSteps from "@/components/content-creation/trick-steps";
import ItemContentLayout from "../../components/item-content-layout";
import Submission from "@/components/submission";

export default {
  components: {Submission, ItemContentLayout},
  data: () => ({
    trick: null,
    difficulty: null
  }),
  methods: {
    ...mapMutations('video-upload', ['activate']),
    edit() {
      this.activate({
        component: TrickSteps, edit: true, editPayload: this.trick
      })
    }
  },
  computed: {
    ...mapState('submissions', ['submissions']),
    ...mapState('tricks', ['dictionary']),
    relatedData() {
      return [
        {
          title: "Categories",
          data: this.trick.categories.map(x => this.dictionary.categories[x]),
          idFactory: c => `category-${c.id}`,
          routeFactory: c => `/category/${c.id}`,
        },
        {
          title: "Prerequisites",
          data: this.trick.prerequisites.map(x => this.dictionary.tricks[x]),
          idFactory: t => `trick-${t.id}`,
          routeFactory: t => `/trick/${t.slug}`,
        },
        {
          title: "Progressions",
          data: this.trick.progressions.map(x => this.dictionary.tricks[x]),
          idFactory: t => `trick-${t.id}`,
          routeFactory: t => `/trick/${t.slug}`,
        },
      ]
    },
  },
  async fetch() {
    const trickSlug = this.$route.params.trick;
    this.trick = this.dictionary.tricks[trickSlug]
    this.difficulty = this.dictionary.difficulties[this.trick.difficulty]
    await this.$store.dispatch("submissions/fetchSubmissionsForTrick", {trickId: trickSlug}, {root: true})
  },
  head() {
    if (!this.trick) return {}

    return {
      title: this.trick.name,
      meta: [
        {hid: 'description', name: 'description', content: this.trick.description}
      ]
    }
  }
}
</script>

<style scoped></style>
