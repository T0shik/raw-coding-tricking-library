<template>
  <div>
    <v-text-field label="Search" placeholder="e.g. cork/flip/kick" v-model="filter"
                  prepend-inner-icon="mdi-magnify" outlined></v-text-field>
    <v-row justify="center">
      <v-col class="d-flex justify-center align-start"
             lg="3"
             v-for="trick in filteredTricks" :key="`trick-info-card-${trick.id}`">
        <trick-info-card :trick="trick" link/>
      </v-col>
    </v-row>
  </div>
</template>

<script>
import TrickInfoCard from "@/components/trick-info-card";
import {hasOccurrences} from "@/data/functions";

export default {
  name: "trick-list",
  components: {TrickInfoCard},
  props: {
    tricks: {
      required: true,
      type: Array,
    }
  },
  data: () => ({
    filter: "",
  }),
  computed: {
    filteredTricks() {
      if (!this.filter) return this.tricks

      return this.tricks.filter(t => {
        let searchIndex = (t.name + t.description).toLowerCase()
        return hasOccurrences(searchIndex, this.filter)
      })
    }
  }
}
</script>

<style scoped>

</style>
