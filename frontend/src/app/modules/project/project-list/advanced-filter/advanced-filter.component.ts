import {Component, OnInit} from '@angular/core';
import {Store} from "@ngrx/store";
import {AdvancedFilterState} from "../../../../core/store/advanced-filter/advancedFilter.reducers";
import {
  updateEndFrom, updateEndTo,
  updateLeader,
  updateMember,
  updateStartFrom,
  updateStartTo
} from "../../../../core/store/advanced-filter/advancedFilter.actions";
import {AdvancedFilter} from "../../../../core/models/filter.models";
import {selectFilterProperties} from "../../../../core/store/advanced-filter/advancedFilter.selectors";
import { SubscriptionService } from 'src/app/core/services/subscription.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-advanced-filter',
  templateUrl: './advanced-filter.component.html',
  styleUrls: ['./advanced-filter.component.scss']
})
export class AdvancedFilterComponent implements OnInit {

  subscriptions: Subscription[] = []
  advancedFilter: AdvancedFilter = {
    leaderName: '',
    memberName: '',
    startDateRange: {
      from: '',
      to: ''
    },
    endDateRange:  {
      from: '',
      to: ''
    }
  }

  constructor(
    protected store: Store<{advancedFilter: AdvancedFilterState}>,
    private subService: SubscriptionService
  ) {}

  ngOnInit() {
    const advancedFilterSub = this.store.select(selectFilterProperties).subscribe(value =>
      this.advancedFilter = {
      ...value,
      startDateRange: {...value.startDateRange},
      endDateRange: {...value.endDateRange}
    })

    this.subscriptions.push(advancedFilterSub)
  }

  ngOnDestroy(): void {
    this.subService.unsubscribe(this.subscriptions)
  }

  updateFilter(field: AdvancedFilterFieldName) {
    switch (field) {
      case AdvancedFilterFieldName.LeaderName:
        this.store.dispatch(updateLeader({leaderName: this.advancedFilter.leaderName}))
        break
      case AdvancedFilterFieldName.MemberName:
        this.store.dispatch(updateMember({memberName: this.advancedFilter.memberName}))
        break
      case AdvancedFilterFieldName.StartDateFrom:
        this.store.dispatch(updateStartFrom({startFrom: this.advancedFilter.startDateRange.from}))
        break
      case AdvancedFilterFieldName.StartDateTo:
        this.store.dispatch(updateStartTo({startTo: this.advancedFilter.startDateRange.to}))
        break
      case AdvancedFilterFieldName.EndDateFrom:
        this.store.dispatch(updateEndFrom({endFrom: this.advancedFilter.endDateRange.from}))
        break
      case AdvancedFilterFieldName.EndDateTo:
        this.store.dispatch(updateEndTo({endTo: this.advancedFilter.endDateRange.to}))
        break
    }
  }

  protected readonly AdvancedFilterFieldName = AdvancedFilterFieldName;
}

export enum AdvancedFilterFieldName {
  LeaderName,
  MemberName,
  StartDateFrom,
  StartDateTo,
  EndDateFrom,
  EndDateTo
}
