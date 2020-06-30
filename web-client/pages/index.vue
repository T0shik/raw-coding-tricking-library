<template>
  <div>

    <div v-if="tricks">
      <div v-for="t in tricks">
        {{t.id}} - {{t.name}}
      </div>
    </div>

    <div v-if="submissions">
      <div v-for="s in submissions">
        {{s.id}} - {{s.description}} - {{s.trickId}}
        <div>
          <video width="400" controls :src="`http://localhost:5000/api/videos/${s.video}`"></video>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
  import {mapState} from 'vuex';

  export default {
    computed: {
      ...mapState('tricks', ['tricks']),
      ...mapState('submissions', ['submissions']),
    }
  }
</script>
