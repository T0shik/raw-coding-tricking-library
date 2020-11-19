<template>
  <div>
    <div>
      <v-text-field label="Email" :disabled="loading" v-model="email">
        <template slot="append-outer">
          <v-btn color="primary" :disabled="loading" :loading="loading" @click="sendInvite">Invite</v-btn>
        </template>
      </v-text-field>
    </div>
    <v-list>
      <v-list-item v-for="user in moderators" :key="user.id">
        <v-list-item-content>
          <v-list-item-title>{{ user.email }}</v-list-item-title>
          <v-list-item-subtitle>{{ user.id }}</v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
    </v-list>
  </div>
</template>

<script>
export default {
  name: "index",
  data: () => ({
    moderators: [],
    email: "",
    loading: false,
  }),
  middleware: ["admin"],
  fetch() {
    return this.$axios.$get("/api/admin/moderators")
      .then(moderators => this.moderators = moderators)
  },
  methods: {
    sendInvite() {
      if (this.loading) return
      this.loading = true
      const data = {
        email: this.email,
        returnUrl: location.origin
      }
      return this.$axios.$post("/api/admin/moderators", data)
        .then(() => {
          this.email = ""
          this.loading = false
        })
        .finally(this.$fetch)
    }
  }
}
</script>

<style scoped>

</style>
