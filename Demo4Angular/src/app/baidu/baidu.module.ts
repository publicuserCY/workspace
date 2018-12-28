import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BaiduRoutingModule } from './baidu-routing.module';
import { BaiduMapComponent } from './baidu-map/baidu-map.component';
import { NgZorroAntdModule } from 'ng-zorro-antd';

@NgModule({
  declarations: [
    BaiduMapComponent
  ],
  imports: [
    CommonModule,
    NgZorroAntdModule,
    BaiduRoutingModule
  ]
})
export class BaiduModule { }
