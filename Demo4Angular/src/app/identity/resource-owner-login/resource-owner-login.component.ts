import { Component, OnInit } from '@angular/core';
import { OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';

import { roAuthConfig } from '../auth-config';

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
  }

  login() {
    this.oAuthService.fetchTokenUsingPasswordFlowAndLoadUserProfile('admin', 'LO3tGX&6').then(() => {
      const claims: any = this.oAuthService.getIdentityClaims();
      if (claims) { console.log('id', claims.sub); }
    });
  }

  refresh() {
    this.oAuthService.refreshToken().then((obj) => {
      const token = this.oAuthService.getRefreshToken();
      console.log('refresh ok');
    });
  }
}
