import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PageNotFoundComponent } from './home/page-not-found/page-not-found.component';

const routes: Routes = [
  { path: 'authority', loadChildren: './authority/authority.module#AuthorityModule' },
  { path: 'baidu', loadChildren: './baidu/baidu.module#BaiduModule' },
  { path: 'identity', loadChildren: './identity/identity.module#IdentityModule' },
  { path: 'sample', loadChildren: './sample/sample.module#SampleModule' },
  { path: '', redirectTo: '/authority', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes,
    {
      enableTracing: false
    })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
