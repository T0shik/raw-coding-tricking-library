const initState = () => ({
  user: null,
  profile: null,
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
  saveProfile(state, {profile}) {
    state.profile = profile
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
      .then(async (user) => {
        if (user) {
          commit('saveUser', {user})
          this.$axios.setToken(`Bearer ${user.access_token}`)
          const profile = await this.$axios.$get('/api/users/me')
          commit('saveProfile', {profile})
        }
      })
      .catch(err => {
        if (err.message === "login_required") {
          return this.$auth.removeUser();
        }
      })
      .finally(() => commit('finish'))
  },
  login() {
    if (process.server) return;
    localStorage.setItem('post-login-redirect-path', location.pathname)
    return this.$auth.signinRedirect();
  },
  _waitAuthenticated({state, getters}) {
    if (process.server) return;

    return new Promise((resolve, reject) => {

      if (state.loading) {
        const unwatch = this.watch(
          (s) => s.auth.loading,
          (n, o) => {
            unwatch();
            resolve(getters.authenticated)
          }
        )

      } else {
        resolve(getters.authenticated)
      }
    })
  }
}
