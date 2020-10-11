import {mapActions, mapState} from 'vuex';

export const close = {
  methods: {
    ...mapActions('video-upload', ['cancelUpload']),
    close() {
      this.cancelUpload()
    }
  }
}

export const form = (formFactory) => ({
  data: () => ({
    form: formFactory()
  }),
  created: function () {
    this.setup(this.form)
  },
  computed: {
    ...mapState('video-upload', ['setup']),
  }
})
