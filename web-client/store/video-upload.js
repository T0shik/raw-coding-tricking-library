const initState = () => ({
  uploadPromise: null,
  active: false,
  component: null
})

export const state = initState

export const mutations = {
  activate(state, {component}) {
    state.active = true;
    state.component = component
  },
  hide(state) {
    state.active = false;
  },
  setTask(state, {uploadPromise}) {
    state.uploadPromise = uploadPromise
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  startVideoUpload({commit, dispatch}, {form}) {
    const uploadPromise = this.$axios.$post("/api/videos", form);
    commit("setTask", {uploadPromise})
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
