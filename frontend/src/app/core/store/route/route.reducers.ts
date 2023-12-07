import {createReducer, on} from "@ngrx/store";
import {switchRoute} from "./route.actions";
import {routes} from "../../constants/routeConstants";

export interface RouteState {
  route: string
}

const initialState: RouteState = {
  route: routes.PROJECT_LIST
}

export const routeReducer = createReducer(
  initialState,
  on(switchRoute, (state, {route}) => ({...state, route}))
)
