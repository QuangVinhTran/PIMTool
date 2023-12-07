import {createAction} from "@ngrx/store";

export enum SettingActionTypes {
  TOGGLE_DARK_MODE = '[Setting Action] Toggle dark mode',
  TOGGLE_FILE_IMPORT = '[Setting Action] Toggle file import allowance',
  TOGGLE_FILE_EXPORT = '[Setting Action] Toggle file export allowance',
  TOGGLE_MULTIPLE_LANGUAGES = '[Setting Action] Toggle multiple languages allowance',
}

export const toggleDarkMode = createAction(
  SettingActionTypes.TOGGLE_DARK_MODE
)

export const toggleFileImport = createAction(
  SettingActionTypes.TOGGLE_FILE_IMPORT
)

export const toggleFileExport = createAction(
  SettingActionTypes.TOGGLE_FILE_EXPORT
)

export const toggleMultiLanguages = createAction(
  SettingActionTypes.TOGGLE_MULTIPLE_LANGUAGES
)
