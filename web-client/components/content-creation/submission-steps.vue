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

        <v-stepper-step step="3">Review</v-stepper-step>
      </v-stepper-header>
      <v-stepper-items class="fpt-0">
        <v-stepper-content step="1">
          <div>
            <v-file-input v-model="file"
                          accept="video/*"
                          @change="handleFile"></v-file-input>
          </div>
        </v-stepper-content>

        <v-stepper-content step="2">
          <v-form ref="form" v-model="validation.valid">
            <v-autocomplete :items="lists.tricks.map(x => ({value: x.slug, text: x.name}))"
                            :rules="validation.trickId"
                            v-model="form.trickId"
                            label="Select Trick"></v-autocomplete>

            <v-text-field label="Description"
                          :rules="validation.description"
                          v-model="form.description"></v-text-field>

            <div class="d-flex justify-center">
              <v-btn :disabled="!validation.valid" @click="$refs.form.validate() ? step++ : 0">Next</v-btn>
            </div>
          </v-form>
        </v-stepper-content>

        <v-stepper-content step="3">
          <div><strong>File Name:</strong> {{ fileName }}</div>
          <div v-if="form.trickId"><strong>Trick:</strong> {{ dictionary.tricks[form.trickId].name }}</div>
          <div><strong>Description:</strong> {{ form.description }}</div>

          <div class="d-flex mt-3">
            <v-btn @click="restart">Restart</v-btn>
            <v-btn class="mx-2" @click="step--">Edit</v-btn>
            <v-spacer/>
            <v-btn color="primary" @click="save">Complete</v-btn>
          </div>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
  </v-card>
</template>

<script>
import {mapActions, mapMutations, mapState} from 'vuex';
import {close, form} from "@/components/content-creation/_shared";

const initForm = () => ({
  trickId: "",
  video: "",
  description: ""
})

export default {
  name: "submission-steps",
  mixins: [
    close,
    form(initForm)
  ],
  data: () => ({
    step: 1,
    file: null,
    validation: {
      valid: false,
      trickId: [v => !!v || "Trick is required."],
      description: [v => !!v || "Description is required."],
    }
  }),
  computed: {
    ...mapState('tricks', ['lists', 'dictionary']),
    fileName() {
      return this.file ? this.file.name : ""
    }
  },
  methods: {
    ...mapMutations('content-update', ['hide']),
    ...mapActions('content-update', ['startVideoUpload', 'createSubmission']),
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
    },
    restart() {
      this.form = initForm()
      this.cancelUpload({hard: false})
      this.step = 1
      this.file = null
    }
  }
}
</script>

<style scoped>

</style>
