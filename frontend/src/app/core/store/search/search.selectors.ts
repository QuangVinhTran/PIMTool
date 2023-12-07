import {createFeatureSelector, createSelector} from "@ngrx/store";
import {SearchCriteria} from "../../models/filter.models";

export const selectSearchCriteria = createFeatureSelector<SearchCriteria>('searchCriteria')

export const selectConjunctionSearch = createSelector(
  selectSearchCriteria,
  (state: SearchCriteria) => state.ConjunctionSearchInfos
)

export const selectDisjunctionSearch = createSelector(
  selectSearchCriteria,
  (state: SearchCriteria) => state.DisjunctionSearchInfos
)

export const selectAllSearch = createSelector(
  selectSearchCriteria,
  (state: SearchCriteria) => state
)
