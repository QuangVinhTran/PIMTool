import {createFeatureSelector, createSelector} from "@ngrx/store";
import {AdvancedFilterState} from "./advancedFilter.reducers";

export const selectAdvancedFilter = createFeatureSelector<AdvancedFilterState>('advancedFilter')

export const selectFilterStatus = createSelector(
  selectAdvancedFilter,
  (state: AdvancedFilterState) => state.showFilter
)

export const selectFilterProperties = createSelector(
  selectAdvancedFilter,
  (state: AdvancedFilterState) => state.filterState
)
