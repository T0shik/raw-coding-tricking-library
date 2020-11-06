<template>
  <div>
    <div>
      <h3 class="text-h5 text-center">Tricks</h3>
      <front-page-trick-feed/>
    </div>
    <v-divider class="my-5"></v-divider>
    <div v-for="s in sections">
      <div class="d-flex flex-column align-center">
        <p class="text-h5">{{ s.title }}</p>
        <div>
          <v-btn class="mx-1" v-for="item in s.collection"
                 :key="`${s.title}-${item.id}`"
                 :to="s.routeFactory(item)">{{ item.name }}
          </v-btn>
        </div>
      </div>
      <v-divider class="my-5"></v-divider>
    </div>
  </div>
</template>

<script>
import {mapState} from 'vuex';
import FrontPageTrickFeed from "@/components/front-page/front-page-trick-feed";

export default {
  components: {FrontPageTrickFeed},
  computed: {
    ...mapState('tricks', ['lists']),
    sections() {
      return [
        // {collection: this.lists.tricks, title: "Tricks", routeFactory: (i) => `/trick/${i.slug}`},
        {collection: this.lists.categories, title: "Categories", routeFactory: (i) => `/category/${i.id}`},
        {collection: this.lists.difficulties, title: "Difficulties", routeFactory: (i) => `/difficulty/${i.id}`},
      ]
    }
  }
}
</script>
