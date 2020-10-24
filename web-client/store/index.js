const initState = () => ({})

export const state = initState

export const mutations = {
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  async nuxtServerInit({dispatch}) {
    await dispatch("auth/initialize")
    await dispatch("tricks/fetchTricks")
  }
}
