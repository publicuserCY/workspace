import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SampleAComponent } from './sample-a/sample-a.component';
import { SampleADetailComponent } from './sample-a/sample-a-detail.component';
import { SampleBComponent } from './sample-b/sample-b.component';
import { SampleRootComponent } from './sample-root/sample-root.component';

const routes: Routes = [
  { path: 'root', component: SampleRootComponent, data: { breadcrumb: 'root' } },
  { path: 'root/A', component: SampleAComponent, data: { breadcrumb: 'A' } },
  { path: 'root/A/:id', component: SampleADetailComponent, data: { breadcrumb: 'A Detail' } },
  { path: 'root/B', component: SampleBComponent, data: { breadcrumb: 'B' } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SampleRoutingModule { }
