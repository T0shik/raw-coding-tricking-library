import {mapActions, mapState} from 'vuex';
import {EVENTS} from "@/data/events";

export const close = {
  methods: {
    ...mapActions('content-creation', ['cancelUpload']),
    close() {
      return this.cancelUpload({hard: true})
    }
  }
}

export const form = (formFactory) => ({
  data: () => ({
    form: formFactory()
  }),
  created: function () {
    if (this.setup)
      this.setup(this.form)
  },
  methods: {
    broadcastUpdate(){
      this.$nuxt.$emit(EVENTS.CONTENT_UPDATED)
      this.loadContent()
    },
    ...mapActions('library', ['loadContent'])
  },
  computed: {
    ...mapState('content-creation', ['setup']),
  }
})
