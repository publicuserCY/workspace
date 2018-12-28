import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BaiduMapComponent } from './baidu-map/baidu-map.component';

const routes: Routes = [
  {
    path: '',
    children: [
      // { path: '', component: BaiduMapComponent },
      { path: 'map', component: BaiduMapComponent },
      { path: '', redirectTo: 'map', pathMatch: 'prefix' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BaiduRoutingModule { }
