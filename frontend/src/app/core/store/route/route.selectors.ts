import {createFeatureSelector, createSelector} from "@ngrx/store";
import {RouteState} from "./route.reducers";

export const selectRoute = createFeatureSelector<RouteState>('route')

export const selectCurrentRoute = createSelector(
  selectRoute,
  (state: RouteState) => state.route
)
