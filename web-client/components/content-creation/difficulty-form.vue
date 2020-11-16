<template>
  <v-card>
    <v-card-title>
      Create Diffculty
      <v-spacer></v-spacer>
      <v-btn icon @click="close">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-card-title>
    <v-card-text>
      <v-form ref="form" v-model="validation.valid">
        <v-text-field :rules="validation.name"
                      label="Name"
                      v-model="form.name"></v-text-field>
        <v-text-field :rules="validation.description"
                      label="Description"
                      v-model="form.description"></v-text-field>
      </v-form>
    </v-card-text>
    <v-card-actions class="d-flex justify-center">
      <v-btn :disabled="!validation.valid" color="primary" @click="$refs.form.validate() ? save() : 0">Create</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import {close} from "./_shared";
import {mapState} from "vuex";

export default {
  name: "difficulty-form",
  mixins: [close],
  data: () => ({
    form: {
      name: "",
      description: "",
    },
    validation: {
      valid: false,
      name: [v => !!v || "Name is required."],
      description: [v => !!v || "Description is required."],
    }
  }),
  created() {
    if (this.editPayload) {
      const {id, name, description} = this.editPayload
      Object.assign(this.form, {id, name, description})
    }
  },
  methods: {
    save() {
      if (this.form.id) {
        this.$axios.put("/api/difficulties", this.form)
      } else {
        this.$axios.post("/api/difficulties", this.form)
      }
      this.close()
    }
  },
  computed: mapState('content-update', ['editPayload'])
}
</script>

<style scoped>

</style>
