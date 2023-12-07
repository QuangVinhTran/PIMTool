import {createAction, props} from "@ngrx/store";

export enum AdvancedFilterActionTypes {
  SHOW_ADVANCED_FILTER = '[Advanced Filter Action] Show advanced filter',
  RESET_ADVANCED_FILTER = '[Advanced Filter Action] Reset advanced filter',
  UPDATE_LEADER = '[Advanced Filter Action] Update leader',
  UPDATE_MEMBER = '[Advanced Filter Action] Update member',
  UPDATE_START_FROM = '[Advanced Filter Action] Update start date from',
  UPDATE_START_TO = '[Advanced Filter Action] Update start date to',
  UPDATE_END_FROM = '[Advanced Filter Action] Update end date from',
  UPDATE_END_TO = '[Advanced Filter Action] Update end date to'
}

export const showAdvancedFilter = createAction(
  AdvancedFilterActionTypes.SHOW_ADVANCED_FILTER
)

export const resetAdvancedFilter = createAction(
  AdvancedFilterActionTypes.RESET_ADVANCED_FILTER
)

export const updateLeader = createAction(
  AdvancedFilterActionTypes.UPDATE_LEADER,
  props<{leaderName: string}>()
)

export const updateMember = createAction(
  AdvancedFilterActionTypes.UPDATE_MEMBER,
  props<{memberName: string}>()
)

export const updateStartFrom = createAction(
  AdvancedFilterActionTypes.UPDATE_START_FROM,
  props<{startFrom: string}>()
)

export const updateStartTo = createAction(
 AdvancedFilterActionTypes.UPDATE_START_TO,
  props<{startTo: string}>()
)

export const updateEndFrom = createAction(
  AdvancedFilterActionTypes.UPDATE_END_FROM,
  props<{endFrom: string}>()
)
export const updateEndTo = createAction(
  AdvancedFilterActionTypes.UPDATE_END_TO,
  props<{endTo: string}>()
)


