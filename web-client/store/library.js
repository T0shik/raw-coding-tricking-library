const initState = () => ({
  dictionary: {
    tricks: null,
    categories: null,
    difficulties: null,
  },
  lists: {
    tricks: [],
    categories: [],
    difficulties: [],
  }
})

export const state = initState

const setEntities = (state, type, data) => {
  state.dictionary[type] = {}
  state.lists[type] = []
  data.forEach(x => {
    state.lists[type].push(x)
    state.dictionary[type][x.id] = x
    state.dictionary[type][x.slug] = x
  })
}

export const mutations = {
  setTricks(state, {tricks}) {
    setEntities(state, 'tricks', tricks)
  },
  setDifficulties(state, {difficulties}) {
    setEntities(state, 'difficulties', difficulties)
  },
  setCategories(state, {categories}) {
    setEntities(state, 'categories', categories)
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  loadContent({commit}) {
    return Promise.all([
      this.$axios.$get("/api/tricks").then(tricks => commit('setTricks', {tricks})),
      this.$axios.$get("/api/difficulties").then(difficulties => commit('setDifficulties', {difficulties})),
      this.$axios.$get("/api/categories").then(categories => commit('setCategories', {categories})),
    ])
  },
  createTrick({state, commit, dispatch}, {form}) {
    return this.$axios.$post("/api/tricks", form)
  },
  updateTrick({state, commit, dispatch}, {form}) {
    return this.$axios.$put("/api/tricks", form)
  }
}
