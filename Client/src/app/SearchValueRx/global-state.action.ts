import { createAction, props } from '@ngrx/store';

export const setSearchValue = createAction(
  '[Global State] Set Search Value',
  props<{ searchValue: string }>()
);

export const setStatus = createAction(
  '[Global State] Set Status',
  props<{ status: string }>()
);
