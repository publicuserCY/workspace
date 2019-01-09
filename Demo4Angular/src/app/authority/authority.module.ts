import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { AuthorityRoutingModule } from './authority-routing.module';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { ApiResourceDetailComponent } from './api-resource/api-resource-detail.component';
import { ApiScopeComponent } from './api-resource/api-scope/api-scope.component';
import { ApiSecretComponent } from './api-resource/api-secret/api-secret.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { ClientsComponent } from './clients/clients.component';
import { UsersComponent } from './users/users.component';
import { BaseService } from '../shared/base.service';
import { PaginatedService } from '../shared/paginated.service';
import { AuthorityInteractionService } from './services/authority-Interaction.service';
import { ApiResourceService } from './services/api-resource.service';


@NgModule({
  declarations: [
    ApiResourceComponent,
    ApiResourceDetailComponent,
    ApiScopeComponent,
    ApiSecretComponent,
    IdentityResourceComponent,
    ClientsComponent,
    UsersComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgZorroAntdModule,
    AuthorityRoutingModule
  ],
  providers: [
    BaseService,
    PaginatedService,
    ApiResourceService,
    AuthorityInteractionService
  ]
})
export class AuthorityModule { }
