import { createAction } from "@ngrx/store";

export enum PageActionTypes {
    SET_LOADING_ON = '[Page Actions] Set loading on',
    SET_LOADING_OFF = '[Page Actions] Set loading off',
}

export const setLoadingOn = createAction(
    PageActionTypes.SET_LOADING_ON
)

export const setLoadingOff = createAction(
    PageActionTypes.SET_LOADING_OFF
)