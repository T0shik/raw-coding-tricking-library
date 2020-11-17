<template>
  <v-card>
    <v-card-title>
      Create Category
      <v-spacer></v-spacer>
      <v-btn icon @click="close">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-card-title>
    <v-card-text>
      <v-form ref="form" v-model="validation.valid">
        <v-text-field :rules="validation.name"
                      label="Name"
                      :disabled="!!editPayload"
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
import {form, close} from "@/components/content-creation/_shared";
import {mapState} from "vuex";

export default {
  name: "category-form",
  mixins: [close, form(() => ({
    name: "",
    description: "",
  }))],
  data: () => ({
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
    async save() {
      if (this.form.id) {
        await this.$axios.put("/api/categories", this.form)
      } else {
        await this.$axios.post("/api/categories", this.form)
      }
      this.broadcastUpdate()
      this.close()
    }
  },
  computed: mapState('content-creation', ['editPayload'])
}
</script>

<style scoped>

</style>
