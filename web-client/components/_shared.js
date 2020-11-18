import {EVENTS} from "@/data/events";

export const onContentUpdate = {
  created() {
    this.$nuxt.$on(EVENTS.CONTENT_UPDATED, this.onContentUpdate)
  },
  destroyed() {
    this.$nuxt.$off(EVENTS.CONTENT_UPDATED, this.onContentUpdate)
  },
  methods: {
    onContentUpdate() {

    }
  }
}
