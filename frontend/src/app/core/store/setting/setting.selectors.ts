import {createFeatureSelector, createSelector} from "@ngrx/store";
import {SettingState} from "./setting.reducers";

export const settingSelector = createFeatureSelector<SettingState>('settings')

export const selectAllSettings = createSelector(
  settingSelector,
  (state: SettingState) => state
)

export const selectDarkMode = createSelector(
  settingSelector,
  (state: SettingState) => state.darkMode
)

export const selectAllowImportFile = createSelector(
  settingSelector,
  (state: SettingState) => state.allowImportFile
)

export const selectAllowExportFile = createSelector(
  settingSelector,
  (state: SettingState) => state.allowExportFile
)

export const selectAllowMultiLanguages = createSelector(
  settingSelector,
  (state: SettingState) => state.allowMultipleLanguages
)
