import { createFeatureSelector, createSelector } from '@ngrx/store';
import { SearchSortModel } from './search-sort.model';

const selection = createFeatureSelector<SearchSortModel[]>('searchSortEntries');

export const selectSearchText = createSelector(
  selection,
  (state: SearchSortModel[]) => {
    const found = state.find(e => e.name == 'searchText');
    if (found) {
      return found.value;
    } else {
      return '';
    }
  }
);
export const selectSearchStatus = createSelector(
  selection,
  (state: SearchSortModel[]) => {
    const found = state.find(e => e.name == 'searchStatus');
    if (found) {
      return found.value;
    } else {
      return '0';
    }
  }
);
export const selectSortNumber = createSelector(
  selection,
  (state: SearchSortModel[]) => {
    const found = state.find(e => e.name == 'sortNumber');
    if (found) {
      return found.value;
    } else {
      return '0';
    }
  }
);
export const selectSortName = createSelector(
  selection,
  (state: SearchSortModel[]) => {
    const found = state.find(e => e.name == 'sortName');
    if (found) {
      return found.value;
    } else {
      return '0';
    }
  }
);
export const selectSortStatus = createSelector(
  selection,
  (state: SearchSortModel[]) => {
    const found = state.find(e => e.name == 'sortStatus');
    if (found) {
      return found.value;
    } else {
      return '0';
    }
  }
);
export const selectSortCustomer = createSelector(
  selection,
  (state: SearchSortModel[]) => {
    const found = state.find(e => e.name == 'sortCustomer');
    if (found) {
      return found.value;
    } else {
      return '0';
    }
  }
);
export const selectSortStartDate = createSelector(
  selection,
  (state: SearchSortModel[]) => {
    const found = state.find(e => e.name == 'sortStartDate');
    if (found) {
      return found.value;
    } else {
      return '0';
    }
  }
);
export const selectCurrentIndexPage = createSelector(
  selection,
  (state: SearchSortModel[]) => {
    const found = state.find(e => e.name == 'currentIndexPage');
    if (found) {
      return parseInt(found.value);
    } else {
      return 1;
    }
  }
);
