import {Component, EventEmitter, Output, OnInit, OnDestroy} from '@angular/core';
import {SearchCriteria, SearchInfo} from "../../../../core/models/filter.models";
import {Store} from "@ngrx/store";
import {
  addConjunctionSearchInfo, addDisjunctionSearchInfo,
  clearConjunctionSearchInfo,
  clearDisjunctionSearchInfo
} from "../../../../core/store/search/search.actions";
import {resetSortInfo} from "../../../../core/store/sort/sort.actions";
import {resetAdvancedFilter, showAdvancedFilter} from "../../../../core/store/advanced-filter/advancedFilter.actions";
import { Subscription } from 'rxjs';
import { SubscriptionService } from 'src/app/core/services/subscription.service';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss']
})
export class SearchBarComponent implements OnInit, OnDestroy {
  @Output() searchProjectEvent = new EventEmitter<void>()
  projectStatus = ''
  currentProjectStatus = ''
  searchKeyword = ''
  subscriptions: Subscription[] = []

  constructor(
    protected store: Store<{searchCriteria: SearchCriteria}>,
    private subService: SubscriptionService
  ) { }
  
  ngOnInit(): void {
    const searchCriteriaSub1 = this.store.select('searchCriteria').subscribe(value => {
      const disjunctionSearchInfos = value.DisjunctionSearchInfos.filter(searchInfo => searchInfo.fieldName == 'status')
      if (disjunctionSearchInfos.length > 0) {
        this.currentProjectStatus = disjunctionSearchInfos[0].value
      }
    })
    const searchCriteriaSub2 = this.store.select('searchCriteria').subscribe(value => {
      const conjunctionSearchInfos = value.ConjunctionSearchInfos.filter(searchInfo => searchInfo.fieldName === 'name')
      if (conjunctionSearchInfos.length > 0) {
        this.searchKeyword = conjunctionSearchInfos[0].value
      }
    })
    
    this.subscriptions.push(searchCriteriaSub1)
    this.subscriptions.push(searchCriteriaSub2)
  }

  ngOnDestroy(): void {
    this.subService.unsubscribe(this.subscriptions)
  }

  protected search(){
    this.store.dispatch(clearConjunctionSearchInfo())
    this.store.dispatch(clearDisjunctionSearchInfo())
    this.store.dispatch(addDisjunctionSearchInfo({
      searchInfo: {
        fieldName: 'status',
        value: this.projectStatus || this.currentProjectStatus
      }
    }))

    const setSearchInfo = () => {
      const searchInfo: SearchInfo = {
        fieldName: 'projectNumber',
        value: this.searchKeyword
      }
      this.store.dispatch(addConjunctionSearchInfo({searchInfo}))
      this.store.dispatch(addConjunctionSearchInfo({searchInfo: {...searchInfo, fieldName: 'name'}}))
      this.store.dispatch(addConjunctionSearchInfo({searchInfo: {...searchInfo, fieldName: 'customer'}}))
    }

    this.searchKeyword && setSearchInfo()
    this.searchProjectEvent.emit()
  }

  protected resetSearch(){
    this.store.dispatch(clearDisjunctionSearchInfo())
    this.store.dispatch(clearConjunctionSearchInfo())
    this.store.dispatch(resetSortInfo())
    this.store.dispatch(resetAdvancedFilter())
    this.projectStatus = ''
    this.currentProjectStatus = ''
    this.searchKeyword = ''
    this.searchProjectEvent.emit()
  }

  protected readonly showAdvancedFilter = showAdvancedFilter;
}
