import { Injectable } from "@angular/core";

@Injectable({
    providedIn : 'root'
})
export class SearchSortState {
    currentPageIndex = 1;
    sortNumber = '0';
    sortName ='0';
    sortStatus = '0';
    sortCustomer = '0';
    sortStartDate = '0';
    searchText = '';
    searchStatus = '0';
}