<template>
  <v-snackbar :value="message" timeout="4000">
    <v-icon :color="popupConfig.colour">{{ popupConfig.icon }}</v-icon>
    {{ message }}
  </v-snackbar>
</template>

<script>
import {mapState} from "vuex";
import {POPUP_TYPES} from "@/store/popup";

export default {
  name: "popup",
  computed: {
    ...mapState('popup', ['message', 'type']),
    popupConfig() {
      if (this.type === POPUP_TYPES.ERROR)
        return {icon: 'mdi-alert-circle', colour: 'red'}
      if (this.type === POPUP_TYPES.SUCCESS)
        return {icon: 'mdi-check', colour: 'green'}
      return {icon: '', colour: ''}
    }
  }
}
</script>

<style scoped>

</style>
