import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { RouterNavigatedAction, ROUTER_NAVIGATION } from '@ngrx/router-store';
import { map } from 'rxjs/operators';
import {Store} from "@ngrx/store";
import {switchRoute} from "./route.actions";
import {AppState} from "../app.state";

@Injectable()
export class RouteEffects {
  constructor(private actions$: Actions, private store: Store<AppState>) {}

  handleRouteChange$ = createEffect(() =>
    this.actions$.pipe(
      ofType<RouterNavigatedAction<any>>(ROUTER_NAVIGATION),
      map((action) => {
        const url = action.payload.routerState.url;
        this.store.dispatch(switchRoute({route: url}))
        return { type: 'ROUTE_CHANGED', payload: { url } };
      })
    )
  );
}
