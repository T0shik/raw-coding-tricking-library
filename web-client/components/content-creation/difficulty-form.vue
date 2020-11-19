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
                      :disabled="staged"
                      v-model="form.name"></v-text-field>
        <v-text-field :rules="validation.description"
                      label="Description"
                      v-model="form.description"></v-text-field>
      </v-form>
    </v-card-text>
    <v-card-actions class="d-flex justify-center">
      <v-btn :disabled="!validation.valid" color="primary" @click="$refs.form.validate() ? save() : 0">
        {{ !!editPayload ? "Update" : "Create" }}
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import {mapState} from "vuex";
import {form, close} from "@/components/content-creation/_shared";
import {VERSION_STATE} from "@/components/moderation";

export default {
  name: "difficulty-form",
  mixins: [close, form(() => ({
    name: "",
    description: "",
  }))],
  data: () => ({
    validation: {
      valid: false,
      name: [v => !!v || "Name is required."],
      description: [v => !!v || "Description is required."],
    },
    staged: false
  }),
  created() {
    if (this.editPayload) {
      const {id, name, description, state} = this.editPayload
      Object.assign(this.form, {id, name, description})
      if (state) {
        this.staged = state === VERSION_STATE.STAGED
      }
    }
  },
  methods: {
    async save() {
      if (this.form.id) {
        if (this.staged) {
          await this.$axios.put("/api/difficulties/staged", this.form)
        } else {
          await this.$axios.put("/api/difficulties", this.form)
        }
      } else {
        await this.$axios.post("/api/difficulties", this.form)
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
