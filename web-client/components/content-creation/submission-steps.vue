<template>
  <v-card>
    <v-card-title>
      Create Submission
      <v-spacer></v-spacer>
      <v-btn icon @click="close">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-card-title>
    <v-stepper class="rounded-0" v-model="step">
      <v-stepper-header class="elevation-0">
        <v-stepper-step :complete="step > 1" step="1">Upload Video</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="step > 2" step="2">Select Trick</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="step > 3" step="3">Submission</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step step="4">Review</v-stepper-step>
      </v-stepper-header>
      <v-stepper-items class="fpt-0">
        <v-stepper-content step="1">
          <div>
            <v-file-input accept="video/*" @change="handleFile"></v-file-input>
          </div>
        </v-stepper-content>

        <v-stepper-content step="2">
          <div>
            <v-select :items="lists.tricks.map(x => ({value: x.slug, text: x.name}))" v-model="form.trickId"
                      label="Select Trick"></v-select>
            <div class="d-flex justify-center">
              <v-btn @click="step++">Next</v-btn>
            </div>
          </div>
        </v-stepper-content>

        <v-stepper-content step="3">
          <div>
            <v-text-field label="Description" v-model="form.description"></v-text-field>
            <div class="d-flex justify-center">
              <v-btn @click="step++">Next</v-btn>
            </div>
          </div>
        </v-stepper-content>

        <v-stepper-content step="4">
          <div class="d-flex justify-center">
            <v-btn @click="save">Save</v-btn>
          </div>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
  </v-card>
</template>

<script>
import {mapActions, mapMutations, mapState} from 'vuex';
import {close, form} from "@/components/content-creation/_shared";

export default {
  name: "submission-steps",
  mixins: [
    close,
    form(() => ({
      trickId: "",
      video: "",
      description: ""
    }))
  ],
  data: () => ({
    step: 1,
  }),
  computed: mapState('tricks', ['lists']),
  methods: {
    ...mapMutations('video-upload', ['hide']),
    ...mapActions('video-upload', ['startVideoUpload', 'createSubmission']),
    async handleFile(file) {
      if (!file) return;

      const form = new FormData();
      form.append("video", file)
      this.startVideoUpload({form});
      this.step++;
    },
    save() {
      this.createSubmission({form: this.form})
      this.hide();
    }
  }
}
</script>

<style scoped>

</style>
