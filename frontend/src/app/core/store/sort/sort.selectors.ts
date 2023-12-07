import {createFeatureSelector, createSelector} from "@ngrx/store";
import {SortState} from "./sort.reducers";

export const selectSort = createFeatureSelector<SortState>('sortInfo')

export const selectSortInfo = createSelector(
  selectSort,
  (state: SortState) => state.sort
)
