import { NgModule } from '@angular/core';

import { PIMBaseModule } from '@base';
import { ProjectListComponent } from './components';
import { ProjectRoutingModule } from './project-routing.module';

@NgModule({
    declarations: [ProjectListComponent],
    providers: [],
    imports: [ProjectRoutingModule, PIMBaseModule]
})
export class ProjectModule {

}
