export default function ({store, redirect}){
  if(!store.getters["auth/moderator"]){
    redirect('/')
  }
}
