import {Component, OnDestroy} from '@angular/core';
import { CommonModule } from '@angular/common';
import {NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {TopBarComponent} from "./core/components/top-bar/top-bar.component";
import {BodyComponent} from "./core/components/body/body.component";
import {HttpClient} from "@angular/common/http";
import {FormsModule} from "@angular/forms";
import {ErrorComponent} from "./core/components/error/error.component";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, TopBarComponent, BodyComponent, FormsModule, ErrorComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnDestroy{
  title = 'PimToolFE';

  showTopBarAndBody: boolean = true;
  evenSubscription?: Subscription;

  constructor(private router: Router) {
    this.evenSubscription = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        // Check if the current route is an error route
        this.showTopBarAndBody = event.url !== '/error';
      }
    });
  }

  ngOnDestroy(): void {
    this.evenSubscription?.unsubscribe();
  }
}
