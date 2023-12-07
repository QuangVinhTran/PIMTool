import {SettingState} from "../store/setting/setting.reducers";

export const getLocalAccessToken = () => {
  return localStorage.getItem('access_token')
}

export const getLocalRefreshToken = () => {
  return localStorage.getItem('refresh_token')
}

export const getLocalSettings = () => {
  let settings: SettingState = {
    darkMode: false,
    allowImportFile: false,
    allowExportFile: false,
    allowMultipleLanguages: false
  }
  try {
    const settingsStr = localStorage.getItem('settings')
    if (settingsStr) {
      settings = JSON.parse(settingsStr)
    }
  } catch (e) {
    console.error(e)
  }
  localStorage.setItem('settings', JSON.stringify(settings))
  return settings
}

export const setLocalSettings = (settings: SettingState) => {
  localStorage.setItem('settings', JSON.stringify(settings))
}

export const getLocalLanguage = (): string => {
  return localStorage.getItem('lang') || ''
}
