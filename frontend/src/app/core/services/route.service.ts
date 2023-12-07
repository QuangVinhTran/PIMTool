import { Injectable } from '@angular/core';
import {NavigationEnd, Router} from "@angular/router";
import {Subscription} from "rxjs";
import {Store} from "@ngrx/store";
import {switchRoute} from "../store/route/route.actions";

@Injectable({
  providedIn: 'root'
})
export class RouteService {
  private routerSubscription!: Subscription

  constructor(private router: Router, private store: Store) { }

  subscribeToRouteChanges() {
    this.routerSubscription = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        const currentRoute = this.router.url.slice(1, this.router.url.length)
        this.store.dispatch(switchRoute({route: currentRoute}))
      }
    })
  }

  unsubscribeToRouteChanges() {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe()
    }
  }
}
