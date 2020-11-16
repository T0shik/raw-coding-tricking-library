<template>
  <div>
    <slot v-if="authenticated" name="allowed"
          :moderator="moderator"
          :admin="admin"></slot>
    <slot v-else name="forbidden" :login="login"></slot>
  </div>
</template>

<script>
import {mapActions, mapGetters} from "vuex";

export default {
  name: "if-auth",
  computed: mapGetters('auth', ['authenticated', 'moderator', 'admin']),
  methods: mapActions('auth', ['login'])
}
</script>

<style scoped>

</style>
