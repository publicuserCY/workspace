import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthorityRoutingModule } from './authority-routing.module';
import { ClientsComponent } from './clients/clients.component';
import { UsersComponent } from './users/users.component';

@NgModule({
  declarations: [
    ClientsComponent,
    UsersComponent
  ],
  imports: [
    CommonModule,
    AuthorityRoutingModule
  ]
})
export class AuthorityModule { }
