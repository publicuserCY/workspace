import { Injectable } from '@angular/core';
// import { UserManager, User } from 'oidc-client';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginServiceService {
  environment = {
    production: false,
    apiUrlBase: '/api',
    openIdConnectSettings: {
      authority: 'https://localhost:5000/',
      client_id: 'odic.client',
      redirect_uri: 'http://localhost:4200/signin-oidc',
      scope: 'openid profile resapi',
      response_type: 'id_token token',
      post_logout_redirect_uri: 'http://localhost:4200/',
      automaticSilentRenew: true,
      silent_redirect_uri: 'http://localhost:4200/redirect-silentrenew'
    }
  };
  /* private userManager: UserManager = new UserManager(this.environment.openIdConnectSettings);
  private currentUser: User;

  userLoaded$ = new ReplaySubject<boolean>(1);

  get userAvailable(): boolean {
    return this.currentUser != null;
  }

  get user(): User {
    return this.currentUser;
  }

  constructor() {
    this.userManager.clearStaleState();

    this.userManager.events.addUserLoaded(user => {
      if (!this.environment.production) {
        console.log('User loaded.', user);
      }
      this.currentUser = user;
      this.userLoaded$.next(true);
    });

    this.userManager.events.addUserUnloaded((e) => {
      if (!this.environment.production) {
        console.log('User unloaded');
      }
      this.currentUser = null;
      this.userLoaded$.next(false);
    });
  }

  triggerSignIn() {
    this.userManager.signinRedirect().then(() => {
      if (!this.environment.production) {
        console.log('Redirection to signin triggered.');
      }
    });
  }

  handleCallback() {
    this.userManager.signinRedirectCallback().then(user => {
      if (!this.environment.production) {
        console.log('Callback after signin handled.', user);
      }
    });
  }

  handleSilentCallback() {
    this.userManager.signinSilentCallback().then(user => {
      this.currentUser = user;
      if (!this.environment.production) {
        console.log('Callback after silent signin handled.', user);
      }
    });
  }

  triggerSignOut() {
    this.userManager.signoutRedirect().then(resp => {
      if (!this.environment.production) {
        console.log('Redirection to sign out triggered.', resp);
      }
    });
  } */
}
