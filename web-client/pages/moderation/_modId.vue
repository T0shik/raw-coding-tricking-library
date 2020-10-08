<template>
  <div>
    <v-row>
      <v-col cols="8">
        <v-row justify="center">
          <v-col cols="4" v-if="current">
            <trick-info-card :trick="current" />
          </v-col>
          <v-col cols="4" class="d-flex justify-center" v-if="current">
            <v-icon size="46">mdi-arrow-right</v-icon>
          </v-col>
          <v-col cols="4" v-if="target">
            <trick-info-card :trick="target" />
          </v-col>
        </v-row>
        <comment-section :comments="comments" @send="sendComment"/>
      </v-col>
      <v-col cols="4">
        <v-card>
          <v-card-title>Reviews ({{ approveCount }} / 3)</v-card-title>
          <v-card-text>
            <div v-if="reviews.length > 0">
              <div v-for="review in reviews" :key="`review-${review.id}`">
                <v-icon :color="reviewStatusColor(review.status)">{{ reviewStatusIcon(review.status) }}</v-icon>
                Username
                <span v-if="review.comment">- {{ review.comment }}</span>
              </div>
            </div>
            <div v-else>No Reviews</div>

            <v-divider class="my-3"></v-divider>

            <v-text-field label="Review Comment" v-model="reviewComment"></v-text-field>
          </v-card-text>
          <div v-if="outdated">
            Outdated
          </div>
          <v-card-actions v-else class="justify-center">
            <v-btn v-for="action in reviewActions"
                   :color="reviewStatusColor(action.status)"
                   :key="`ra-${action.title}`"
                   :disabled="action.disabled"
                   @click="createReview(action.status)">
              <v-icon>{{ reviewStatusIcon(action.status) }}</v-icon>
              {{ action.title }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script>

import CommentSection from "@/components/comments/comment-section";
import TrickInfoCard from "@/components/trick-info-card";

const endpointResolver = (type) => {
  if (type === 'trick') return 'tricks'
}

const REVIEW_STATUS = {
  APPROVED: 0,
  REJECTED: 1,
  WAITING: 2,
}

const reviewStatusColor = (status) => {
  if (REVIEW_STATUS.APPROVED === status) return "green"
  if (REVIEW_STATUS.REJECTED === status) return "red"
  if (REVIEW_STATUS.WAITING === status) return "orange"
  return ''
}

const reviewStatusIcon = (status) => {
  if (REVIEW_STATUS.APPROVED === status) return "mdi-check"
  if (REVIEW_STATUS.REJECTED === status) return "mdi-close"
  if (REVIEW_STATUS.WAITING === status) return "mdi-clock"
  return ''
}

export default {
  components: {TrickInfoCard, CommentSection},
  data: () => ({
    current: null,
    target: null,
    comments: [],
    reviews: [],
    reviewComment: "",
    replyId: 0,
  }),
  async created() {
    const {modId} = this.$route.params

    const modItem = await this.$axios.$get(`/api/moderation-items/${modId}`)
    this.comments = modItem.comments
    this.reviews = modItem.reviews

    const endpoint = endpointResolver(modItem.type)

    this.$axios.$get(`/api/${endpoint}/${modItem.current}`)
      .then((item) => this.current = item)
    this.$axios.$get(`/api/${endpoint}/${modItem.target}`)
      .then((item) => this.target = item)
  },
  methods: {
    sendComment(content) {
      const {modId} = this.$route.params

      return this.$axios.$post(`/api/moderation-items/${modId}/comments`,
        {content})
        .then((comment) => this.comments.push(comment))
    },
    createReview(status) {
      const {modId} = this.$route.params

      return this.$axios.$post(`/api/moderation-items/${modId}/reviews`,
        {
          comment: this.reviewComment,
          status: status,
        })
        .then((review) => this.reviews.push(review))
    },
    reviewStatusColor,
    reviewStatusIcon,
  },
  computed: {
    reviewActions() {
      return [
        {title: "Approve", status: REVIEW_STATUS.APPROVED, disabled: false},
        {title: "Reject", status: REVIEW_STATUS.REJECTED, disabled: !this.reviewComment},
        {title: "Wait", status: REVIEW_STATUS.WAITING, disabled: !this.reviewComment},
      ]
    },
    approveCount() {
      return this.reviews.filter(x => x.status === REVIEW_STATUS.APPROVED).length
    },
    outdated() {
      return this.current && this.target && this.target.version - this.current.version <= 0
    }
  }
}
</script>

<style scoped>

</style>
