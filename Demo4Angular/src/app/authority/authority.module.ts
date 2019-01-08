import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { AuthorityRoutingModule } from './authority-routing.module';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { ApiResourceDetailComponent } from './api-resource/api-resource-detail.component';
// import { ApiScopeComponent } from './api-resource/api-scope/api-scope.component';
import { ApiSecretComponent } from './api-resource/api-secret/api-secret.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { ClientsComponent } from './clients/clients.component';
import { UsersComponent } from './users/users.component';
import { ApiSecretService } from './services/api-secret.service';
import { ApiResourceService } from './services/api-resource.service';
import { AuthorityInteractionService } from './services/authority-Interaction.service';


@NgModule({
  declarations: [
    ApiResourceComponent,
    ApiResourceDetailComponent,
    // ApiScopeComponent,
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
    ApiSecretService,
    ApiResourceService,
    AuthorityInteractionService
  ]
})
export class AuthorityModule { }
