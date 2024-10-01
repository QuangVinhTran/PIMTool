import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { PIMBaseModule } from '../Base/base.module';
import { ShellComponent } from './components/shell/shell.component';

@NgModule({
    declarations: [
        ShellComponent
    ],
    imports: [
        RouterModule,
        PIMBaseModule
    ]
})
export class ShellModule {

}
