<template>
  <div class="d-flex justify-center align-start">
    <div class="mx-2" v-if="submissions">
      <div v-for="x in 1">

        <div v-for="s in submissions">
          {{s.id}} - {{s.description}} - {{s.trickId}}
          <div>
            <video width="400" controls :src="`http://localhost:5000/api/videos/${s.video}`"></video>
          </div>
        </div>
      </div>
    </div>

    <div class="mx-2 sticky">
      <v-sheet class="pa-3 mt-2">
        <div class="text-h6">{{ trick.name }}</div>
        <v-divider class="my-1"></v-divider>
        <div class="text-body-2">{{ trick.description }}</div>
        <div class="text-body-2">{{ trick.difficulty }}</div>
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
  </div>
</template>

<script>
  import {mapState, mapGetters} from 'vuex';

  export default {
    computed: {
      ...mapState('submissions', ['submissions']),
      ...mapState('tricks', ['categories', 'tricks']),
      ...mapGetters('tricks', ['trickById']),
      trick() {
        return this.trickById(this.$route.params.trick)
      },
      relatedData() {
        return [
          {
            title: "Categories",
            data: this.categories.filter(x => this.trick.categories.indexOf(x.id) >= 0),
            idFactory: c => `category-${c.id}`,
            routeFactory: c => `/`,
          },
          {
            title: "Prerequisites",
            data: this.tricks.filter(x => this.trick.prerequisites.indexOf(x.id) >= 0),
            idFactory: t => `trick-${t.id}`,
            routeFactory: t => `/tricks/${t.id}`,
          },
          {
            title: "Progressions",
            data: this.tricks.filter(x => this.trick.progressions.indexOf(x.id) >= 0),
            idFactory: t => `trick-${t.id}`,
            routeFactory: t => `/tricks/${t.id}`,
          },
        ]
      },
    },
    async fetch() {
      const trickId = this.$route.params.trick;
      await this.$store.dispatch("submissions/fetchSubmissionsForTrick", {trickId}, {root: true})
    },
    head () {
      return {
        title: this.trick.name,
        meta: [
          { hid: 'description', name: 'description', content: this.trick.description }
        ]
      }
    }
  }
</script>

<style scoped>
  .sticky {
    position: -webkit-sticky;
    position: sticky;
    top: 48px;
  }
</style>
