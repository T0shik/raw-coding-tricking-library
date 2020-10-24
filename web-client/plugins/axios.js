export default function({$axios}){
  $axios.setHeader('X-Requested-With', 'XMLHttpRequest')
  $axios.onRequest(config => {
    config.withCredentials = true
  })
}
