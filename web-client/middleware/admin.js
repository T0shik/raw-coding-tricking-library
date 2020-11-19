export default function ({store, redirect}) {
  if (!store.getters["auth/admin"]) {
    redirect('/')
  }
}
