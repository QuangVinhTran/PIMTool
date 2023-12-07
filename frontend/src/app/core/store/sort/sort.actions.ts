import {createAction, props} from "@ngrx/store";

export enum SortActionTypes {
  ADD_SORT_INFO = '[Sort Action] Add sort info',
  REMOVE_SORT_INFO = '[Sort Action] Remove sort info',
  REVERT_SORT_ORDER = '[Sort Action] Revert sort order',
  RESET_SORT_INFO = '[Sort Action] Reset sort info',
}

export const addSortInfo = createAction(
  SortActionTypes.ADD_SORT_INFO,
  props<{fieldName: string}>()
)

export const removeSortInfo = createAction(
  SortActionTypes.REMOVE_SORT_INFO,
  props<{fieldName: string}>()
)

export const revertSortOrder = createAction(
  SortActionTypes.REVERT_SORT_ORDER,
  props<{fieldName: string}>()
)

export const resetSortInfo = createAction(
  SortActionTypes.RESET_SORT_INFO
)
