<template>
  <div>
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
    <if-auth>
      <template v-slot:allowed>
        <div>
          <v-btn @click="edit(); close();" outlined small>edit</v-btn>
          <v-btn @click="upload(); close();" outlined small>upload</v-btn>
        </div>
      </template>
      <template v-slot:forbidden="{login}">
        <div class="d-flex justify-center">
          <v-btn small outlined @click="login">
            Log in to edit/update
          </v-btn>
        </div>
      </template>
    </if-auth>
    <v-divider class="mt-2"></v-divider>
    <user-header class="pa-2" :username="trick.user.username" :image-url="trick.user.image" reverse>
      <template v-slot:append>
        <span>{{ trick.version === 1 ? 'Created by' : 'Edited by' }}</span>
      </template>
    </user-header>
  </div>
</template>

<script>
import {mapMutations, mapState} from "vuex";
import TrickSteps from "@/components/content-creation/trick-steps";
import SubmissionSteps from "@/components/content-creation/submission-steps";
import UserHeader from "@/components/user-header";
import IfAuth from "@/components/auth/if-auth";

export default {
  name: "trick-info-card",
  components: {IfAuth, UserHeader},
  props: {
    trick: {
      required: true,
      type: Object,
    },
    close: {
      required: false,
      type: Function,
      defaults: () => {
      }
    }
  },
  methods: {
    ...mapMutations('video-upload', ['activate']),
    edit() {
      this.activate({
        component: TrickSteps, edit: true, editPayload: this.trick
      })
    },
    upload() {
      this.activate({
        component: SubmissionSteps,
        setup: (form) => form.trickId = this.trick.slug
      })
    }
  },
  computed: {
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
    difficulty() {
      return this.dictionary.difficulties[this.trick.difficulty]
    }
  },
}
</script>

<style scoped>

</style>
