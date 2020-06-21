<template>
  <v-layout
    column
    justify-center
    align-center
  >
    <v-flex
      xs12
      sm8
      md6
    >
      <div class="text-center">
        <logo/>
        <vuetify-logo/>
      </div>

      <div v-if="tricks">
        <p v-for="t in tricks">
          {{t.name}}
        </p>
      </div>

      <div>
        <v-text-field label="Tricking Name" v-model="trickName"></v-text-field>
        <v-btn @click="saveTrick">Save Trick</v-btn>

      </div>

      {{message}}
      <v-btn @click="reset">Reset Message</v-btn>
      <v-btn @click="resetTricks">Reset Tricks</v-btn>

    </v-flex>
  </v-layout>
</template>

<script>
  import Logo from '~/components/Logo.vue'
  import VuetifyLogo from '~/components/VuetifyLogo.vue'
  import Axios from "axios";
  import {mapState, mapActions, mapMutations} from 'vuex';

  export default {
    components: {
      Logo,
      VuetifyLogo
    },
    data: () => ({
      trickName: ""
    }),
    computed: {
      ...mapState({
        message: state => state.message
      }),
      ...mapState('tricks', {
        tricks: state => state.tricks
      })
    },
    methods: {
      ...mapMutations([
        'reset'
      ]),
      ...mapMutations('tricks', {
        resetTricks: 'reset'
      }),
      ...mapActions('tricks', ['createTrick']),
      async saveTrick() {
        await this.createTrick({trick: {name: this.trickName}});
        this.trickName = ""
      }
    }
    // async fetch (){
    //   await this.$store.dispatch('fetchMessage');
    // }
    // asyncData(payload){
    //   console.log("asyncData called")
    //   return Axios.get("http://localhost:5000/api/home")
    //     .then(({data}) => {
    //       return { message: data }
    //     })
    // }
  }
</script>
