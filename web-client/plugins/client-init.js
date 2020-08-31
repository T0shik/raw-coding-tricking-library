import {UserManager, WebStorageStateStore} from "oidc-client";

export default async ({app, store}, inject) => {
  const userManager = new UserManager({
    authority: "https://localhost:5001",
    client_id: "web-client",
    redirect_uri: "https://localhost:3000/oidc/sign-in-callback.html",
    response_type: "code",
    scope: 'openid profile IdentityServerApi role',
    post_logout_redirect_uri: "https://localhost:3000",
    // silent_redirect_uri: "https://localhost:3000/",
    userStore: new WebStorageStateStore({store: window.localStorage})
  })
  inject('auth', userManager)

  app.fetch = () => {
    return store.dispatch('clientInit')
  }
}
