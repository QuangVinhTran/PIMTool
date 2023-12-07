import {createAction, props} from "@ngrx/store";
import {SearchInfo} from "../../models/filter.models";

export enum SearchActionTypes {
  ADD_CONJUNCTION_SEARCH_INFO = '[Search Action] Add conjunction search info',
  ADD_DISJUNCTION_SEARCH_INFO = '[Search Action] Add disjunction search info',
  CLEAR_CONJUNCTION_SEARCH_INFO = '[Search Action] Clear conjunction search infos',
  CLEAR_DISJUNCTION_SEARCH_INFO = '[Search Action] Clear disjunction search infos'
}

export const addConjunctionSearchInfo = createAction(
  SearchActionTypes.ADD_CONJUNCTION_SEARCH_INFO,
  props<{searchInfo: SearchInfo}>()
)

export const addDisjunctionSearchInfo = createAction(
  SearchActionTypes.ADD_DISJUNCTION_SEARCH_INFO,
  props<{searchInfo: SearchInfo}>()
)

export const clearConjunctionSearchInfo = createAction(
  SearchActionTypes.CLEAR_CONJUNCTION_SEARCH_INFO
)

export const clearDisjunctionSearchInfo = createAction(
  SearchActionTypes.CLEAR_DISJUNCTION_SEARCH_INFO
)
