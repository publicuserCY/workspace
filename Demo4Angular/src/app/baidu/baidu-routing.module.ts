import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BaiduMapComponent } from './baidu-map/baidu-map.component';

const routes: Routes = [
  {
    path: '',
    children: [
      // { path: '', component: BaiduMapComponent },
      { path: 'Map', component: BaiduMapComponent, data: { breadcrumb: '百度地图' } },
      { path: '', redirectTo: 'Map', pathMatch: 'prefix' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BaiduRoutingModule { }
