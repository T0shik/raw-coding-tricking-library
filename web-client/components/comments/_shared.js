export const COMMENT_PARENT_TYPE = {
  MODERATION_ITEM: 0,
  SUBMISSION: 1,
  COMMENT: 2,
}

export const configurable = {
  props: {
    parentId: {
      type: Number,
      required: true
    },
    parentType: {
      type: Number,
      required: true
    }
  }
}

export const creator = {
  methods: {
    emitComment(comment) {
      this.$emit('comment-created', comment)
    },
    cancel() {
      this.$emit('cancel')
    }
  }
}

export const container = {
  data: () => ({
    comments: []
  }),
  methods: {
    appendComment(comment) {
      this.comments.push(comment)
    },
    prependComment(comment) {
      this.comments.unshift(comment)
    }
  }
}
