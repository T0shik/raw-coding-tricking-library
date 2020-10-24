<template>
  <v-app dark>
    <v-app-bar app dense>
      <nuxt-link class="text-h5 text--primary" style="text-decoration: none;" to="/">Tricking Library</nuxt-link>

      <v-spacer></v-spacer>

      <v-btn class="mx-1" depressed to="/moderation">Moderation</v-btn>

      <if-auth>
        <template v-slot:allowed>
          <div>
            <content-creation-dialog/>
            <v-menu offset-y>
              <template v-slot:activator="{ on, attrs }">
                <v-btn icon v-bind="attrs" v-on="on">
                  <v-avatar size="36">
                    <img v-if="profile.image" :src="profile.image"
                         alt="profile image"/>
                    <v-icon v-else>mdi-account-circle</v-icon>
                  </v-avatar>
                </v-btn>
              </template>
              <v-list>
                <v-list-item @click="$router.push('/profile')">
                  <v-list-item-title>
                    <v-icon left>mdi-account-circle</v-icon>
                    Profile
                  </v-list-item-title>
                </v-list-item>
                <v-list-item @click="logout">
                  <v-list-item-title>
                    <v-icon left>mdi-logout</v-icon>
                    Logout
                  </v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </div>
        </template>
        <template v-slot:forbidden="{login}">
          <v-btn outlined @click="login">
            <v-icon left>mdi-account-circle-outline</v-icon>
            Log In
          </v-btn>
        </template>
      </if-auth>
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
import {mapActions, mapGetters, mapState} from "vuex";
import IfAuth from "@/components/auth/if-auth";

export default {
  name: "default",
  components: {IfAuth, ContentCreationDialog},
  computed: {
    ...mapState('auth', ['profile']),
    ...mapGetters('auth', ['moderator']),
  },
  methods: {
    logout() {
      console.log("Logout not implemnted")
    }
  }
}
</script>
