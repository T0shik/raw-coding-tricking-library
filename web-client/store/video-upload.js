const initState = () => ({
  uploadPromise: null,
  uploadCancelSource: null,
  uploadCompleted: false,
  active: false,
  component: null,
  editing: false,
  editPayload: null,
})

export const state = initState

export const mutations = {
  activate(state, {component, edit = false, editPayload = null}) {
    state.active = true;
    state.component = component;
    if(edit){
      state.editing = true;
      state.editPayload = editPayload
    }
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
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  startVideoUpload({commit, dispatch}, {form}) {
    const source = this.$axios.CancelToken.source()
    const uploadPromise = this.$axios.post("/api/files", form, {
      progress: false,
      cancelToken: source.token
    })
      .then(({data}) => {
        commit('completeUpload')
        return data
      })
      .catch(err => {
        if(this.$axios.isCancel(err)){
          // todo popup notify
        }
      })

    commit("setTask", {uploadPromise, source})
  },
  async cancelUpload({state, commit}) {
    if (state.uploadPromise) {
      if (state.uploadCompleted) {
        commit('hide')
        const video = await state.uploadPromise
        await this.$axios.delete("/api/files/" + video)
      } else {
        state.uploadCancelSource.cancel()
      }
    }

    commit('reset')
  },
  async createSubmission({state, commit, dispatch}, {form}) {
    if (!state.uploadPromise) {
      console.log("uploadPromise is null")
      return;
    }

    form.video = await state.uploadPromise;
    await dispatch('submissions/createSubmission', {form}, {root: true})
    commit('reset')
  }
}
