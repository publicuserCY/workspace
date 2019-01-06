import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgZorroAntdModule } from 'ng-zorro-antd';

import { SampleRoutingModule } from './sample-routing.module';
import { SampleRootComponent } from './sample-root/sample-root.component';
import { SampleAComponent } from './sample-a/sample-a.component';
import { SampleADetailComponent } from './sample-a/sample-a-detail.component';
import { SampleBComponent } from './sample-b/sample-b.component';

@NgModule({
  declarations: [
    SampleRootComponent,
    SampleAComponent,
    SampleADetailComponent,
    SampleBComponent],
  imports: [
    CommonModule,
    NgZorroAntdModule,
    SampleRoutingModule
  ]
})
export class SampleModule { }
