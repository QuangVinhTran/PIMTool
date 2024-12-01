import { createReducer, on } from '@ngrx/store';
import * as GlobalStateActions from './global-state.action';
import { GlobalState } from './global-state.model';

export const initialState: GlobalState = {
  searchValue: '',
  status: '',
};

export const globalStateReducer = createReducer(
  initialState,
  on(GlobalStateActions.setSearchValue, (state, { searchValue }) => ({
    ...state,
    searchValue,
  })),
  on(GlobalStateActions.setStatus, (state, { status }) => ({
    ...state,
    status,
  }))
);
