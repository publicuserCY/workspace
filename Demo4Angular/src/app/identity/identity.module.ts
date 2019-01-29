import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgZorroAntdModule } from 'ng-zorro-antd';

import { IdentityRoutingModule } from './identity-routing.module';
import { ImplicitLoginComponent } from './implicit-login/implicit-login.component';
import { ResourceOwnerLoginComponent } from './resource-owner-login/resource-owner-login.component';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { ApiResourceDetailComponent } from './api-resource/api-resource-detail.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { ClientComponent } from './clients/client/client.component';
import { UsersComponent } from './users/users.component';
import { BaseService } from '../shared/base.service';
import { PaginatedService } from '../shared/paginated.service';
import { AuthorityInteractionService } from './service/authority-Interaction.service';
import { ApiResourceService } from './service/api-resource.service';
import { ClientService } from './service/client.service';

@NgModule({
  declarations: [
    ImplicitLoginComponent,
    ResourceOwnerLoginComponent,
    ApiResourceComponent,
    ApiResourceDetailComponent,
    IdentityResourceComponent,
    ClientComponent,
    UsersComponent
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
    AuthorityInteractionService,
    ApiResourceService,
    ClientService
  ]
})
export class IdentityModule { }
