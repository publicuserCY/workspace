import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { AuthorityRoutingModule } from './authority-routing.module';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { ApiResourceDetailComponent } from './api-resource/api-resource-detail.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { ClientsComponent } from './clients/clients.component';
import { UsersComponent } from './users/users.component';
import { AuthorityService } from './services/authority.service';
import { UniqueApiResourceNameValidator } from './validator/api-resource-name.validator';

@NgModule({
  declarations: [
    ApiResourceComponent,
    ApiResourceDetailComponent,
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
    AuthorityService,
    UniqueApiResourceNameValidator
  ]
})
export class AuthorityModule { }
