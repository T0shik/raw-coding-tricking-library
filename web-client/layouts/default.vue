<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>Tricking Library</v-toolbar-title>

      <v-spacer></v-spacer>

      <video-upload/>
    </v-app-bar>
    <v-main>
      <nuxt/>
    </v-main>
  </v-app>
</template>

<script>
  import VideoUpload from "../components/video-upload";

  export default {
    components: {VideoUpload},
  }
</script>
