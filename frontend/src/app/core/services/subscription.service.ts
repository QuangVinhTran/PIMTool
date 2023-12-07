import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService {

  constructor() { }

  unsubscribe(subscriptions: Subscription[]): void {
    subscriptions.forEach((sub: Subscription) => sub.unsubscribe())
  }
}
