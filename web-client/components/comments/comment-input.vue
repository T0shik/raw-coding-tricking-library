<template>
  <div>
    <v-text-field
      label="Comment"
      v-model="content"
      clearable
      @keydown.ctrl.enter="send"/>
    <v-btn v-if="$listeners['cancel']" @click="cancel">Cancel</v-btn>
    <v-btn :disabled="!content" @click="send">{{ label }}</v-btn>
  </div>
</template>

<script>
import {configurable, creator} from "@/components/comments/_shared";

export default {
  name: "comment-input",
  mixins: [creator, configurable],
  props: {
    label: {
      required: false,
      type: String,
      default: "send"
    }
  },
  data: () => ({
    content: ""
  }),
  methods: {
    send() {
      const data = {
        parentId: this.parentId,
        parentType: this.parentType,
        content: this.content,
      }

      return this.$axios.$post('/api/comments', data)
        .then(this.emitComment)
        .then(this.cancel)
    }
  }
}
</script>

<style scoped>

</style>
