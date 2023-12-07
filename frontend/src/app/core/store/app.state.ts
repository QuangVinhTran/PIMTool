import {SearchCriteria} from "../models/filter.models";
import {RouteState} from "./route/route.reducers";
import {SortState} from "./sort/sort.reducers";
import {SettingState} from "./setting/setting.reducers";

export interface AppState {
  searchCriteria: SearchCriteria
  route: RouteState
  sort: SortState
  settings: SettingState
}
