import {createReducer, on} from "@ngrx/store";
import {
  resetAdvancedFilter,
  showAdvancedFilter,
  updateEndFrom, updateEndTo,
  updateLeader,
  updateMember,
  updateStartFrom,
  updateStartTo
} from "./advancedFilter.actions";
import {AdvancedFilter} from "../../models/filter.models";

export interface AdvancedFilterState {
  showFilter: boolean,
  filterState: AdvancedFilter
}

const initialState: AdvancedFilterState = {
  showFilter: false,
  filterState: {
    leaderName: '',
    memberName: '',
    startDateRange: {
      from: '',
      to: ''
    },
    endDateRange: {
      from: '',
      to: ''
    }
  }
}

export const advancedFilterReducer = createReducer(
  initialState,
  on(showAdvancedFilter, (state) => ({...state, showFilter: !state.showFilter})),
  on(resetAdvancedFilter, _ => initialState),
  on(updateLeader, (state, {leaderName}) => ({...state, filterState: {...state.filterState, leaderName}})),
  on(updateMember, (state, {memberName}) => ({...state, filterState: {...state.filterState, memberName}})),
  on(updateStartFrom, (state, {startFrom}) => ({...state, filterState: {...state.filterState, startDateRange: {...state.filterState.startDateRange, from: startFrom}}})),
  on(updateStartTo, (state, {startTo}) => ({...state, filterState: {...state.filterState, startDateRange: {...state.filterState.startDateRange, to: startTo}}})),
  on(updateEndFrom, (state, {endFrom}) => ({...state, filterState: {...state.filterState, endDateRange: {...state.filterState.endDateRange, from: endFrom}}})),
  on(updateEndTo, (state, {endTo}) => ({...state, filterState: {...state.filterState, endDateRange: {...state.filterState.endDateRange, to: endTo}}})),
)
