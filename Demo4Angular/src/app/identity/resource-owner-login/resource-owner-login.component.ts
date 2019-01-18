import { Component, OnInit } from '@angular/core';
import { OAuthService, JwksValidationHandler, OAuthErrorEvent, OAuthEvent } from 'angular-oauth2-oidc';

import { roAuthConfig } from '../auth-config';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-resource-owner-login',
  templateUrl: './resource-owner-login.component.html',
  styleUrls: ['./resource-owner-login.component.css']
})
export class ResourceOwnerLoginComponent implements OnInit {

  constructor(private oAuthService: OAuthService) { }

  ngOnInit() {
    this.oAuthService.configure(roAuthConfig);
    this.oAuthService.tokenValidationHandler = new JwksValidationHandler();
    this.oAuthService.setStorage(localStorage);
    this.oAuthService.loadDiscoveryDocument();
    /* this.oAuthService.events.subscribe((e: OAuthEvent) => {
      if (e instanceof OAuthErrorEvent) {
        const reason = e.reason as HttpErrorResponse;
        console.log(reason.error.error + reason.error.error_description);
      }
      console.log('oauth/oidc event', e);
    }); */
  }

  login() {
    this.oAuthService.fetchTokenUsingPasswordFlowAndLoadUserProfile('admin', 'LO3tGX&6').then((obj) => {
      console.log('oauth/oidc event', obj);
    }).catch(reason => {
      console.log(reason.error.error + '  ' + reason.error.error_description);
    });
  }

  public logout() {
    this.oAuthService.logOut();
  }

  refresh() {
    this.oAuthService.refreshToken().then((obj) => {
      this.oAuthService.getRefreshToken();
      console.log('refresh ok');
    });
  }

  public get id() {
    const claims: any = this.oAuthService.getIdentityClaims();
    if (!claims) { return null; }
    return claims.sub;
  }

  public get name() {
    const claims: any = this.oAuthService.getIdentityClaims();
    if (!claims) { return null; }
    return claims.given_name;
  }
}
