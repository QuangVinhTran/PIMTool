import {createReducer, on} from "@ngrx/store";
import {SortInfo} from "../../models/filter.models";
import {addSortInfo, removeSortInfo, resetSortInfo, revertSortOrder} from "./sort.actions";

export interface SortState {
  sort: SortInfo
}

const initialState: SortState = {
  sort: {
    fieldName: 'projectNumber',
    ascending: true
  }
}

export const sortReducer = createReducer(
  initialState,
  on(addSortInfo, (state, {fieldName}) => ({...state, sort: {...state.sort, fieldName}})),
  on(removeSortInfo, (state, {fieldName}) => ({...state, sort: {...state.sort, fieldName: ''}})),
  on(revertSortOrder, (state, {fieldName}) => {
    if (state.sort.ascending) {
      return ({...state, sort: {...state.sort, ascending: false}})
    }
    return initialState
  }),
  on(resetSortInfo, _ => initialState)
)
