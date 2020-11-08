import {mapActions, mapState} from 'vuex';

export const close = {
  methods: {
    ...mapActions('video-upload', ['cancelUpload']),
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
  computed: {
    ...mapState('video-upload', ['setup']),
  }
})
