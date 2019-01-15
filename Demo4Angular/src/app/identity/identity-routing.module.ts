import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ImplicitLoginComponent } from './implicit-login/implicit-login.component';
import { ResourceOwnerLoginComponent } from './resource-owner-login/resource-owner-login.component';

const routes: Routes = [
  { path: 'Implicit', component: ImplicitLoginComponent },
  { path: 'ResourceOwner', component: ResourceOwnerLoginComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityRoutingModule { }
