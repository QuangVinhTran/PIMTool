import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { SubscriptionService } from '../core/services/subscription.service';
import { selectLoadingState } from '../core/store/page/page.selectors';

@Component({
  selector: 'app-default-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class DefaultLayoutComponent implements OnInit, OnDestroy {
  subscriptions: Subscription[] = []
  isLoading: boolean = false
  
  constructor(
    private store: Store,
    private subService: SubscriptionService
  ) {}
  
  ngOnInit(): void {
    this.subscriptions.push(
      this.store.select(selectLoadingState).subscribe(value => this.isLoading = value)
    )
  }

  ngOnDestroy(): void {
    this.subService.unsubscribe(this.subscriptions)
  }
}
