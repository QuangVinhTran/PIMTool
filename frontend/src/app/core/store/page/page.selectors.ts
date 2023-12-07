import { createFeatureSelector, createSelector } from "@ngrx/store";
import { PageState } from "./page.reducer";

const pageSelector = createFeatureSelector<PageState>('page')

export const selectLoadingState = createSelector(
    pageSelector,
    (state: PageState) => state.isLoading
)