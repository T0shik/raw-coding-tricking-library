<template>
  <div>
    <div>
      <v-btn @click="api('test')">Api Test Auth</v-btn>
      <v-btn @click="api('mod')">Api Mod Auth</v-btn>
    </div>
    <div v-for="s in sections">
      <div class="d-flex flex-column align-center">
        <p class="text-h5">{{ s.title }}</p>
        <div>
          <v-btn class="mx-1" v-for="item in s.collection"
                 :key="`${s.title}-${item.slug}`"
                 :to="s.routeFactory(item.slug)">{{ item.name }}
          </v-btn>
        </div>
      </div>
      <v-divider class="my-5"></v-divider>
    </div>
  </div>
</template>

<script>
import {mapState} from 'vuex';

export default {
  methods: {
    api(x) {
      return this.$axios.$get("/api/tricks/" + x)
        .then(msg => console.log(msg));
    }
  },
  computed: {
    ...mapState('tricks', ['tricks', 'categories', 'difficulties']),
    sections() {
      return [
        {collection: this.tricks, title: "Tricks", routeFactory: (id) => `/trick/${id}`},
        {collection: this.categories, title: "Categories", routeFactory: (id) => `/category/${id}`},
        {collection: this.difficulties, title: "Difficulties", routeFactory: (id) => `/difficulty/${id}`},
      ]
    }
  }
}
</script>
