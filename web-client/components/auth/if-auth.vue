<template>
  <div>
    <slot v-if="authenticated" name="allowed"></slot>
    <slot v-else name="forbidden" :login="login"></slot>
  </div>
</template>

<script>
import {mapActions, mapGetters} from "vuex";

export default {
  name: "if-auth",
  computed: mapGetters('auth', ['authenticated']),
  methods: mapActions('auth', ['login'])
}
</script>

<style scoped>

</style>
