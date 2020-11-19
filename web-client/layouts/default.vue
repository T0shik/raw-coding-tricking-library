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
        <template v-slot:allowed="{moderator, admin}">
          <div class="d-flex align-center">
            <content-creation-dialog/>
            <v-menu offset-y>
              <template v-slot:activator="{ on, attrs }">
                <v-btn icon v-bind="attrs" v-on="on">
                  <user-header :image-url="profile.image" :link="false" size="36"/>
                </v-btn>
              </template>
              <v-list>
                <v-list-item v-if="admin" to="/admin">
                  <v-list-item-title>
                    <v-icon left>mdi-cogs</v-icon>
                    Admin Panel
                  </v-list-item-title>
                </v-list-item>
                <v-list-item v-if="moderator" to="/moderation">
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
          <v-btn class="d-none d-md-flex" outlined @click="login">
            <v-icon left>mdi-login</v-icon>
            Log In
          </v-btn>
          <v-btn class="d-flex d-md-none" icon @click="login">
            <v-icon>mdi-login</v-icon>
          </v-btn>
        </template>
      </if-auth>
    </v-app-bar>
    <v-main>
      <v-container>
        <nuxt/>
      </v-container>
    </v-main>
    <popup/>
  </v-app>
</template>

<script>
import ContentCreationDialog from "../components/content-creation/content-creation-dialog";
import {mapActions, mapGetters, mapState} from "vuex";
import IfAuth from "@/components/auth/if-auth";
import NavBarSearch from "@/components/nav-bar-search";
import UserHeader from "@/components/user-header";
import Popup from "@/components/popup";

export default {
  name: "default",
  components: {Popup, UserHeader, NavBarSearch, IfAuth, ContentCreationDialog},
  computed: {
    ...mapState('auth', ['profile']),
    ...mapGetters('auth', ['moderator']),
  },
  methods: mapActions('auth', ['logout']),
}
</script>
