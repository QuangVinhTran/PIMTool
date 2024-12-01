import { createFeatureSelector, createSelector } from '@ngrx/store';
import { GlobalState } from './global-state.model';

const selectGlobalState = createFeatureSelector<GlobalState>('globalState');

export const selectSearchValue = createSelector(
  selectGlobalState,
  (state: GlobalState) => state.searchValue
);

export const selectStatus = createSelector(
  selectGlobalState,
  (state: GlobalState) => state.status
);
