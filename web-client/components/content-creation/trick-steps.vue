<template>
  <v-card>
    <v-card-title>
      Create Trick
      <v-spacer></v-spacer>
      <v-btn icon @click="close">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-card-title>
    <v-stepper class="rounded-0" v-model="step">
      <v-stepper-header class="elevation-0">
        <v-stepper-step :complete="step > 1" step="1">Trick Information</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step step="2">Review</v-stepper-step>
      </v-stepper-header>
      <v-stepper-items class="fpt-0">
        <v-stepper-content step="1">
          <div>
            <v-text-field label="Name" v-model="form.name"></v-text-field>
            <v-text-field label="Description" v-model="form.description"></v-text-field>
            <v-select :items="difficultyItems" v-model="form.difficulty" label="Difficulty"></v-select>
            <v-select :items="trickItems" v-model="form.prerequisites" label="Prerequisites"
                      multiple small-chips chips deletable-chips></v-select>
            <v-select :items="trickItems" v-model="form.progressions" label="Progressions"
                      multiple small-chips chips deletable-chips></v-select>
            <v-select :items="categoryItems" v-model="form.categories" label="Categories"
                      multiple small-chips chips deletable-chips></v-select>
            <div class="d-flex justify-center">
              <v-btn @click="step++">Next</v-btn>
            </div>
          </div>
        </v-stepper-content>

        <v-stepper-content step="2">
          <div class="d-flex justify-center">
            <v-btn @click="save">Save</v-btn>
          </div>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
  </v-card>
</template>

<script>
  import {mapState, mapGetters, mapActions, mapMutations} from 'vuex';
  import {close} from "./_shared";

  export default {
    name: "trick-steps",
    mixins: [close],
    data: () => ({
      step: 1,
      form: {
        name: "",
        description: "",
        difficulty: "",
        prerequisites: [],
        progressions: [],
        categories: [],
      },
      testData: [
        {text: "Foo", value: 1},
        {text: "Bar", value: 2},
        {text: "Baz", value: 3},
      ]
    }),
    computed: {
      ...mapGetters('tricks', ['categoryItems', 'difficultyItems', 'trickItems']),
    },
    methods: {
      ...mapActions('tricks', ['createTrick']),
      async save() {
        await this.createTrick({form: this.form})
        this.close();
      },
    }
  }
</script>

<style scoped>

</style>
