<template>
  <div>
    <v-text-field label="Search" placeholder="e.g. cork/flip/kick" v-model="filter"
                  prepend-inner-icon="mdi-magnify" outlined></v-text-field>
    <div v-for="t in filteredTricks">
      {{t.name}} - {{t.description}}
    </div>
  </div>
</template>

<script>
  export default {
    name: "trick-list",
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

        const normilze = this.filter.trim().toLowerCase();
        return this.tricks.filter(t => t.name.toLowerCase().includes(normilze)
          || t.description.toLowerCase().includes(normilze))
      }
    }
  }
</script>

<style scoped>

</style>
