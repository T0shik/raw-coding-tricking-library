<template>
  <div class="d-flex mt-3 justify-center align-start">
    <div class="mx-3" v-if="submissions">
      <div v-for="x in 10">
        <v-card class="mb-3" v-for="s in submissions" :key="`${x}-${trick.id}-${s.id}`">
          <video-player :video="s.video" :key="`v-${x}-${trick.id}-${s.id}`"/>
          <v-card-text>{{s.description}}</v-card-text>
        </v-card>
      </div>
    </div>

    <v-sheet class="pa-3 sticky">
      <div class="text-h5">
        <span>{{ trick.name }}</span>
        <v-chip class="mb-1 ml-2" small :to="`/difficulty/${difficulty.id}`">
          {{ difficulty.name }}
        </v-chip>
      </div>
      <v-divider class="my-1"></v-divider>
      <div class="text-body-2">{{ trick.description }}</div>
      <v-divider class="my-1"></v-divider>
      <div v-for="rd in relatedData" v-if="rd.data.length > 0">
        <div class="text-subtitle-1">{{rd.title}}</div>
        <v-chip-group>
          <v-chip v-for="d in rd.data" :key="rd.idFactory(d)" small :to="rd.routeFactory(d)">
            {{d.name}}
          </v-chip>
        </v-chip-group>
      </div>
    </v-sheet>
  </div>
</template>

<script>
  // todo: clean up submission id's ^^^
  import {mapState, mapGetters} from 'vuex';
  import VideoPlayer from "../../components/video-player";

  export default {
    components: {VideoPlayer},
    data: () => ({
      trick: null,
      difficulty: null
    }),
    computed: {
      ...mapState('submissions', ['submissions']),
      ...mapState('tricks', ['categories', 'tricks']),
      ...mapGetters('tricks', ['trickById', 'difficultyById']),
      relatedData() {
        return [
          {
            title: "Categories",
            data: this.categories.filter(x => this.trick.categories.indexOf(x.id) >= 0),
            idFactory: c => `category-${c.id}`,
            routeFactory: c => `/category/${c.id}`,
          },
          {
            title: "Prerequisites",
            data: this.tricks.filter(x => this.trick.prerequisites.indexOf(x.id) >= 0),
            idFactory: t => `trick-${t.id}`,
            routeFactory: t => `/trick/${t.id}`,
          },
          {
            title: "Progressions",
            data: this.tricks.filter(x => this.trick.progressions.indexOf(x.id) >= 0),
            idFactory: t => `trick-${t.id}`,
            routeFactory: t => `/trick/${t.id}`,
          },
        ]
      },
    },
    async fetch() {
      const trickId = this.$route.params.trick;
      this.trick = this.trickById(this.$route.params.trick)
      this.difficulty = this.difficultyById(this.trick.difficulty)
      await this.$store.dispatch("submissions/fetchSubmissionsForTrick", {trickId}, {root: true})
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
