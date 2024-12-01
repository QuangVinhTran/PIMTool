import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FilterService {
  private filterValue: string = '';
  private filterStatus: string = '';

  getFilterValue(): string {
    return this.filterValue;
  }

  setFilterValue(value: string): void {
    this.filterValue = value;
  }

  getfilterStatus(): string {
    return this.filterStatus;
  }

  setfilterStatus(value: string): void {
    this.filterStatus = value;
  }
}
