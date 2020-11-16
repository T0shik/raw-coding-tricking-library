<template>
  <v-app dark>
    <v-app-bar app>
      <nuxt-link class="text-h5 text--primary mr-2" style="text-decoration: none;" to="/">
        <span class="d-none d-md-flex">Tricking Library</span>
        <span class="d-flex d-md-none">TL</span>
      </nuxt-link>

      <v-spacer/>
      <nav-bar-search/>
      <v-spacer/>

      <if-auth>
        <template v-slot:allowed="{moderator}">
          <div class="d-flex align-center">
            <v-btn v-if="moderator"
                   class="d-none d-md-flex mx-1"
                   depressed
                   to="/moderation">Moderation</v-btn>
            <content-creation-dialog/>
            <v-menu offset-y>
              <template v-slot:activator="{ on, attrs }">
                <v-btn icon v-bind="attrs" v-on="on">
                  <user-header :image-url="profile.image" :link="false" size="36"/>
                </v-btn>
              </template>
              <v-list>
                <v-list-item class="d-flex d-md-none" to="/moderation">
                  <v-list-item-title>
                    <v-icon left>mdi-clipboard</v-icon>
                    Moderation
                  </v-list-item-title>
                </v-list-item>
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
import NavBarSearch from "@/components/nav-bar-search";
import UserHeader from "@/components/user-header";

export default {
  name: "default",
  components: {UserHeader, NavBarSearch, IfAuth, ContentCreationDialog},
  computed: {
    ...mapState('auth', ['profile']),
    ...mapGetters('auth', ['moderator']),
  },
  methods: mapActions('auth', ['logout']),
}
</script>
