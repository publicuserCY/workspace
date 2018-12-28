import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SampleRoutingModule } from './sample-routing.module';
import { SampleRootComponent } from './sample-root/sample-root.component';
import { SampleAComponent } from './sample-a/sample-a.component';
import { SampleBComponent } from './sample-b/sample-b.component';

@NgModule({
  declarations: [
    SampleRootComponent,
    SampleAComponent,
    SampleBComponent],
  imports: [
    CommonModule,
    SampleRoutingModule
  ]
})
export class SampleModule { }
