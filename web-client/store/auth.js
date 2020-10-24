const initState = () => ({
  profile: null
})

export const state = initState

const ROLES = {
  MODERATOR: "Mod"
}

export const getters = {
  authenticated: state => state.profile != null,
  moderator: (state, getters) => getters.authenticated && state.profile.isMod
}

export const mutations = {
  saveProfile(state, {profile}) {
    state.profile = profile
  }
}

export const actions = {
  initialize({commit}) {
    return this.$axios.$get('/api/users/me')
      .then(profile => commit('saveProfile', {profile}))
      .catch(e => {
        console.error("loading profile error", e.response)
      })
  },
  login() {
    if (process.server) return;
    localStorage.setItem('post-login-redirect-path', location.pathname)
    window.location = this.$config.auth.loginPath
  }
}
