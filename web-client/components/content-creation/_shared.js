import {mapActions} from 'vuex';

export const close = {
  methods: {
    ...mapActions('video-upload', ['cancelUpload']),
    close() {
      this.cancelUpload()
    }
  }
}
