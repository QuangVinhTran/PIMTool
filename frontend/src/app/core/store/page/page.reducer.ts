import { createReducer, on } from "@ngrx/store"
import { setLoadingOff, setLoadingOn } from "./page.actions"

export interface PageState {
    isLoading: boolean
}

const initialState: PageState = {
    isLoading: false
}

export const pageReducer = createReducer(
    initialState,
    on(setLoadingOn, (state: PageState) => ({...state, isLoading: true})),
    on(setLoadingOff, (state: PageState) => ({...state, isLoading: false})),
)