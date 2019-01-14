import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ClientsComponent } from './clients/clients.component';
import { UsersComponent } from './users/users.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { ApiResourceDetailComponent } from './api-resource/api-resource-detail.component';

const routes: Routes = [
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
export class AuthorityRoutingModule { }
