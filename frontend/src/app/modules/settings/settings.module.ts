import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GeneralSettingsComponent } from './general-settings/general-settings.component';
import {SettingsRoutingModule} from "./settings-routing.module";
import { SettingItemComponent } from './components/setting-item/setting-item.component';



@NgModule({
  declarations: [
    GeneralSettingsComponent,
    SettingItemComponent
  ],
  imports: [
    CommonModule,
    SettingsRoutingModule
  ]
})
export class SettingsModule { }
