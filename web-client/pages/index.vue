<template>
  <div>

    <div v-if="tricks">
      <div v-for="t in tricks">
        <v-btn :to="`/trick/${t.id}`">{{t.name}}</v-btn>
      </div>
    </div>

  </div>
</template>

<script>
  import {mapState} from 'vuex';

  export default {
    computed: mapState('tricks', ['tricks'])
  }
</script>
