import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { DefaultComponent } from './default/default.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { NgZorroAntdModule } from 'ng-zorro-antd';

@NgModule({
    declarations: [
        DefaultComponent,
        PageNotFoundComponent
    ],
    imports: [
        CommonModule,
        NgZorroAntdModule,
        HomeRoutingModule
    ]
})
export class HomeModule { }
