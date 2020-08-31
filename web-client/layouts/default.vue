<template>
  <v-app dark>
    <v-app-bar app dense>
      <nuxt-link class="text-h5 text--primary" style="text-decoration: none;" to="/">Tricking Library</nuxt-link>

      <v-spacer></v-spacer>

      <v-btn class="mx-1" v-if="moderator" depressed to="/moderation">Moderation</v-btn>

      <v-skeleton-loader class="mx-1" :loading="loading" transition="fade-transition" type="button">
        <content-creation-dialog></content-creation-dialog>
      </v-skeleton-loader>

      <v-skeleton-loader class="mx-1" :loading="loading" transition="fade-transition" type="button">
        <v-btn depressed outlined v-if="authenticated">
          <v-icon left>mdi-account-circle</v-icon>
          Profile
        </v-btn>
        <v-btn depressed outlined v-else @click="$auth.signinRedirect()">
          <v-icon left>mdi-account-circle-outline</v-icon>
          sign in
        </v-btn>
      </v-skeleton-loader>
      <v-btn v-if="authenticated" depressed @click="$auth.signoutRedirect()">Logout</v-btn>

    </v-app-bar>
    <v-main>
      <v-container>
        <nuxt/>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import ContentCreationDialog from "../components/content-creation/content-creation-dialog";
import {mapGetters, mapState} from "vuex";

export default {
  components: {ContentCreationDialog},
  computed: {
    ...mapState('auth', ['loading']),
    ...mapGetters('auth', ['authenticated', 'moderator']),
  }
}
</script>
