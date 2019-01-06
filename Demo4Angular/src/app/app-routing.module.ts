import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PageNotFoundComponent } from './home/page-not-found/page-not-found.component';

const routes: Routes = [
  { path: 'Authority', loadChildren: './authority/authority.module#AuthorityModule' },
  { path: 'Baidu', loadChildren: './baidu/baidu.module#BaiduModule' },
  { path: 'Identity', loadChildren: './identity/identity.module#IdentityModule' },
  { path: 'Sample', loadChildren: './sample/sample.module#SampleModule' },
  { path: '', redirectTo: '/Authority/ApiResources', pathMatch: 'full' },
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
