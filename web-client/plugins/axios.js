export default function ({$axios, store}) {
  $axios.setHeader('X-Requested-With', 'XMLHttpRequest')
  $axios.onRequest(config => {
    config.withCredentials = true
  })

  $axios.onError(error => {
    if (error.response && error.response.status && error.response.data) {
      const {status, data} = error.response
      if ((status | 0) === 400) {
        store.dispatch('popup/error', data)
        return {error}
      }
    }
  })
}
