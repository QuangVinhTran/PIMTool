import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {GeneralSettingsComponent} from "./general-settings/general-settings.component";

const settingsRoutes: Routes = [
  {
    path: '',
    component: GeneralSettingsComponent
  }
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(settingsRoutes)
  ]
})
export class SettingsRoutingModule { }
