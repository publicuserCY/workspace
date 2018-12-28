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
    this.oAuthService.setStorage(sessionStorage);
    this.oAuthService.loadDiscoveryDocument();
  }

  login() {
    this.oAuthService.fetchTokenUsingPasswordFlowAndLoadUserProfile('admin', 'LO3tGX&6').then(() => {
      const claims: any = this.oAuthService.getIdentityClaims();
      const token = this.oAuthService.getAccessToken();
      if (claims) { console.log('id', claims.sub); }
      console.log('token', token);
    });
    /*     this.oAuthService.fetchTokenUsingPasswordFlow('admin', 'LO3tGX&6').then((resp) => {
          return this.oAuthService.loadUserProfile();
        }).then(() => {
          const claims: any = this.oAuthService.getIdentityClaims();
          if (claims) { console.log('given_name', claims.given_name); }
        });
      } */
  }

  refresh() {
    this.oAuthService.refreshToken().then((obj) => {
      console.log('refresh ok');
    });
  }
}
