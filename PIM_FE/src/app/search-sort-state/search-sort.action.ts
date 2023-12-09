import { createAction, props } from "@ngrx/store";
import { SearchSortModel } from "./search-sort.model";
export const addSearchSort = createAction('Add search sort', props<SearchSortModel>());
export const removeSearchSort = createAction('Remove search sort', props<SearchSortModel>());
export const clearSearchSort = createAction('Clear search sort');