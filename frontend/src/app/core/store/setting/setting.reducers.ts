import {createReducer, on} from "@ngrx/store";
import {getLocalSettings} from "../../utils/localStorage.util";
import {
  toggleDarkMode,
  toggleFileExport,
  toggleFileImport,
  toggleMultiLanguages
} from "./setting.actions";

export interface SettingState {
  darkMode: boolean
  allowImportFile: boolean
  allowExportFile: boolean
  allowMultipleLanguages: boolean
}

const initialState: SettingState = {
  darkMode: false,
  allowImportFile: false,
  allowExportFile: false,
  allowMultipleLanguages: false
}

export const settingReducer = createReducer(
  getLocalSettings() || initialState,
  on(toggleDarkMode, (state) => ({...state, darkMode: !state.darkMode})),
  on(toggleFileImport, (state) => ({...state, allowImportFile: !state.allowImportFile})),
  on(toggleFileExport, (state) => ({...state, allowExportFile: !state.allowExportFile})),
  on(toggleMultiLanguages, (state) => ({...state, allowMultipleLanguages: !state.allowMultipleLanguages})),
)
