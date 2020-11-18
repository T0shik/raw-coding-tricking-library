export const POPUP_TYPES = {
  ERROR: 0,
  SUCCESS: 1,
}

const initState = () => ({
  message: "",
  type: POPUP_TYPES.ERROR
})

export const state = initState

export const mutations = {
  show(state, {message, type}) {
    state.message = message
    state.type = type
  },
  hide(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  error({commit}, message) {
    commit('show', {message, type: POPUP_TYPES.ERROR})
  },
  success({commit}, message) {
    commit('show', {message, type: POPUP_TYPES.SUCCESS})
  },
}
