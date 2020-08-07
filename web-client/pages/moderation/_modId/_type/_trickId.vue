﻿
<template>
  <div>
    <div v-if="item">
      {{item.description}}
    </div>
    <div>
      <v-text-field label="Comment" v-model="comment"></v-text-field>
      <v-btn @click="send">Send</v-btn>
    </div>
    <div v-for="c in comments" v-html="c.htmlContent"></div>
  </div>
</template>

<script>

  const endpointResolver = (type) => {
    if (type === 'trick') return 'tricks'
  }

  export default {
    data: () => ({
      item: null,
      comments: [],
      comment: ""
    }),
    created() {
      const {modId, type, trickId} = this.$route.params
      const endpoint = endpointResolver(type)
      this.$axios.$get(`/api/${endpoint}/${trickId}`)
        .then((item) => this.item = item)
      this.$axios.$get(`/api/moderation-items/${modId}/comments`)
        .then((comments) => this.comments = comments)
    },
    methods: {
      send() {
        const {modId} = this.$route.params

        this.$axios.$post(`/api/moderation-items/${modId}/comments`,
          {content: this.comment})
          .then((comment) => this.comments.push(comment))
      }
    }
  }
</script>

<style scoped>

</style>
