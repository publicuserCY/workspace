import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SampleAComponent } from './sample-a/sample-a.component';
import { SampleBComponent } from './sample-b/sample-b.component';
import { SampleRootComponent } from './sample-root/sample-root.component';

const routes: Routes = [
  {
    path: '',
    children:
      [
        { path: 'a', component: SampleAComponent },
        { path: 'b', component: SampleBComponent },
        { path: '', component: SampleRootComponent }
      ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SampleRoutingModule { }
