import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

import { JwksValidationHandler } from 'angular-oauth2-oidc';
import { implicitAuthConfig } from '../auth-config';

@Component({
  selector: 'app-implicit-login',
  templateUrl: './implicit-login.component.html',
  styleUrls: ['./implicit-login.component.css']
})
export class ImplicitLoginComponent implements OnInit {

  constructor(private oAuthService: OAuthService) { }

  ngOnInit() {
    this.oAuthService.configure(implicitAuthConfig);
    this.oAuthService.tokenValidationHandler = new JwksValidationHandler();
    this.oAuthService.loadDiscoveryDocumentAndTryLogin();
  }
  public login() {
    this.oAuthService.initImplicitFlow();
  }

  public logoff() {
    this.oAuthService.logOut();
  }

  public get name() {
    const claims: any = this.oAuthService.getIdentityClaims();
    if (!claims) { return null; }
    return claims.given_name;
  }
}
