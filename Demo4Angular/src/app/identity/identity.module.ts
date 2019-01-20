import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgZorroAntdModule } from 'ng-zorro-antd';

import { IdentityRoutingModule } from './identity-routing.module';
import { ImplicitLoginComponent } from './implicit-login/implicit-login.component';
import { ResourceOwnerLoginComponent } from './resource-owner-login/resource-owner-login.component';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { ApiResourceDetailComponent } from './api-resource/api-resource-detail.component';
import { ApiScopeComponent } from './api-resource/api-scope/api-scope.component';
import { ApiScopeClaimComponent } from './api-resource/api-scope/api-scope-claim.component';
import { ApiSecretComponent } from './api-resource/api-secret/api-secret.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { ClientsComponent } from './clients/clients.component';
import { UsersComponent } from './users/users.component';
import { BaseService } from '../shared/base.service';
import { PaginatedService } from '../shared/paginated.service';
import { AuthorityInteractionService } from './service/authority-Interaction.service';
import { ApiResourceService } from './service/api-resource.service';
import { ApiScopeService } from './service/api-scope.service';
import { NestableFormDirective } from '../shared/nestable-form.directive';

@NgModule({
  declarations: [
    ImplicitLoginComponent,
    ResourceOwnerLoginComponent,
    ApiResourceComponent,
    ApiResourceDetailComponent,
    ApiScopeComponent,
    ApiScopeClaimComponent,
    ApiSecretComponent,
    IdentityResourceComponent,
    ClientsComponent,
    UsersComponent,
    NestableFormDirective
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgZorroAntdModule,
    IdentityRoutingModule
  ],
  providers: [
    BaseService,
    PaginatedService,
    ApiResourceService,
    ApiScopeService,
    AuthorityInteractionService
  ]
})
export class IdentityModule { }
