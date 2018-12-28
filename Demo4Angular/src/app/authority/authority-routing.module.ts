import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ClientsComponent } from './clients/clients.component';
import { UsersComponent } from './users/users.component';

const routes: Routes = [
  {
    path: '',
    children:
      [
        { path: 'clients', component: ClientsComponent },
        { path: 'users', component: UsersComponent },
        { path: '', redirectTo: 'clients', pathMatch: 'prefix' }
      ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthorityRoutingModule { }
