import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ImplicitLoginComponent } from './implicit-login/implicit-login.component';
import { ResourceOwnerLoginComponent } from './resource-owner-login/resource-owner-login.component';
import { ClientsComponent } from './clients/clients.component';
import { UsersComponent } from './users/users.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { ApiResourceDetailComponent } from './api-resource/api-resource-detail.component';

const routes: Routes = [
  { path: 'Implicit', component: ImplicitLoginComponent },
  { path: 'ResourceOwner', component: ResourceOwnerLoginComponent },
  { path: 'ApiResources', component: ApiResourceComponent },
  { path: 'ApiResources/:id', component: ApiResourceDetailComponent },
  { path: 'IdentityResources', component: IdentityResourceComponent },
  { path: 'Clients', component: ClientsComponent },
  { path: 'Users', component: UsersComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityRoutingModule { }
