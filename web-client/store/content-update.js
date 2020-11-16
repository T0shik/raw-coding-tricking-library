const initState = (active = false, component = null) => ({
  uploadPromise: null,
  uploadCancelSource: null,
  uploadCompleted: false,
  active: active,
  component: component,
  editing: false,
  editPayload: null,
  setup: null
})

export const state = initState

export const mutations = {
  activate(state, {component, edit = false, editPayload = null, setup = null}) {
    state.active = true;
    state.component = component;
    if (edit) {
      state.editing = true;
      state.editPayload = editPayload
    }
    state.setup = setup
  },
  hide(state) {
    state.active = false;
  },
  setTask(state, {uploadPromise, source}) {
    state.uploadPromise = uploadPromise
    state.uploadCancelSource = source
  },
  completeUpload(state) {
    state.uploadCompleted = true
  },
  reset(state, {hard}) {
    if (hard) {
      Object.assign(state, initState())
    } else {
      Object.assign(state, initState(true, state.component))
    }
  }
}

export const actions = {
  startVideoUpload({commit, dispatch}, {form}) {
    const source = this.$axios.CancelToken.source()
    const uploadPromise = this.$axios.$post("/api/files", form, {
      progress: false,
      cancelToken: source.token
    })
      .then(video => {
        commit('completeUpload')
        return video
      })
      .catch(err => {
        if (this.$axios.isCancel(err)) {
          // todo popup notify
        }
      })

    commit("setTask", {uploadPromise, source})
  },
  async cancelUpload({state, commit}, {hard}) {
    if (state.uploadPromise) {
      if (state.uploadCompleted) {
        commit('hide')
        const video = await state.uploadPromise
        await this.$axios.delete("/api/files/" + video)
      } else {
        state.uploadCancelSource.cancel()
      }
    }

    commit('reset', {hard})
  },
  async createSubmission({state, commit, dispatch}, {form}) {
    if (!state.uploadPromise) {
      console.log("uploadPromise is null")
      return;
    }

    form.video = await state.uploadPromise;
    await this.$axios.$post("/api/submissions", form)
    commit('reset', {hard: true})
  }
}
