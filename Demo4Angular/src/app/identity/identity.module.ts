import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IdentityRoutingModule } from './identity-routing.module';
import { ImplicitLoginComponent } from './implicit-login/implicit-login.component';
import { ResourceOwnerLoginComponent } from './resource-owner-login/resource-owner-login.component';

@NgModule({
  declarations: [
    ImplicitLoginComponent,
    ResourceOwnerLoginComponent
  ],
  imports: [
    CommonModule,
    IdentityRoutingModule
  ]
})
export class IdentityModule { }
