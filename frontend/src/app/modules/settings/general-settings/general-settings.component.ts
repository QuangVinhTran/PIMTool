import {Component, OnInit} from '@angular/core';
import {SettingState} from "../../../core/store/setting/setting.reducers";
import {Store} from "@ngrx/store";
import {selectAllSettings} from "../../../core/store/setting/setting.selectors";
import {
  SettingActionTypes,
  toggleDarkMode,
  toggleFileExport,
  toggleFileImport, toggleMultiLanguages
} from "../../../core/store/setting/setting.actions";
import {setLocalSettings} from "../../../core/utils/localStorage.util";

@Component({
  selector: 'app-general-settings',
  templateUrl: './general-settings.component.html',
  styleUrls: ['./general-settings.component.scss']
})
export class GeneralSettingsComponent implements OnInit{

  constructor(protected store: Store) {
  }

  settings!: SettingState

  ngOnInit() {
    this.store.select(selectAllSettings).subscribe(value => this.settings = value)
  }

  toggle(actionType: SettingActionTypes) {
    switch (actionType) {
      case SettingActionTypes.TOGGLE_DARK_MODE:
        this.store.dispatch(toggleDarkMode())
        break
      case SettingActionTypes.TOGGLE_FILE_IMPORT:
        this.store.dispatch(toggleFileImport())
        break
      case SettingActionTypes.TOGGLE_FILE_EXPORT:
        this.store.dispatch(toggleFileExport())
        break
      case SettingActionTypes.TOGGLE_MULTIPLE_LANGUAGES:
        this.store.dispatch(toggleMultiLanguages())
        break
    }
    setLocalSettings(this.settings)
  }

  protected readonly SettingActionTypes = SettingActionTypes;
}
