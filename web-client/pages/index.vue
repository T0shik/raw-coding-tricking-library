<template>
  <div>

    <div v-if="tricks">
      <div v-for="t in tricks">
        {{t.name}}
        <div>
          <video width="400" controls :src="`http://localhost:5000/api/videos/${t.video}`"></video>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
  import {mapState} from 'vuex';

  export default {
    computed: mapState('tricks', ['tricks']),
  }
</script>
