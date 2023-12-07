import {Component, OnInit} from '@angular/core';
import {Store} from "@ngrx/store";
import {routes} from "../../core/constants/routeConstants";
import {selectCurrentRoute} from "../../core/store/route/route.selectors";

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.scss']
})
export class SideBarComponent implements OnInit {
  constructor(private store: Store<{route: string}>) { }

  currentRoute = ''

  ngOnInit() {
    this.store.select(selectCurrentRoute).subscribe(value => this.currentRoute = value)
  }

  protected readonly routes = routes;
}
