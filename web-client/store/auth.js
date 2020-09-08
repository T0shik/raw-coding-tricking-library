const initState = () => ({
  user: null,
  loading: true
})

export const state = initState

const ROLES = {
  MODERATOR: "Mod"
}

export const getters = {
  authenticated: state => !state.loading && state.user != null,
  moderator: (state, getters) => getters.authenticated && state.user.profile.role === ROLES.MODERATOR
}

export const mutations = {
  saveUser(state, {user}) {
    state.user = user
  },
  finish(state) {
    state.loading = false
  }
}

export const actions = {
  initialize({commit}) {
    return this.$auth.querySessionStatus()
      .then(sessionStatus => {
        if (sessionStatus) {
          return this.$auth.getUser()
        }
      })
      .then(user => {
        if (user) {
          commit('saveUser', {user})
          this.$axios.setToken(`Bearer ${user.access_token}`)
        }
      })
      .catch(err => {
        console.log(err.message)
        if (err.message === "login_required") {
          return this.$auth.removeUser();
        }
      })
      .finally(() => commit('finish'))
  }
}
