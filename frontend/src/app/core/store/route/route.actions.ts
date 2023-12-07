import {createAction, props} from "@ngrx/store";

export const switchRoute = createAction(
  '[Route Action] Switch route',
  props<{route: string}>()
)
