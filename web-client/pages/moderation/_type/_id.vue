<template>
  <div>
    <div v-if="item">
      {{item.description}}
    </div>
  </div>
</template>

<script>

  const endpointResolver = (type) => {
    if (type === 'trick') return 'tricks'
  }

  export default {
    data: () => ({
      item: null
    }),
    async fetch() {
      const {type, id} = this.$route.params
      const endpoint = endpointResolver(type)
      this.item = await this.$axios.$get(`/api/${endpoint}/${id}`)
    }
  }
</script>

<style scoped>

</style>
