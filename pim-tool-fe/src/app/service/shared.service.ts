import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SharedService {
  private savedSearchText!: String;
  private savedSatus!: String;
  private isUpdate: boolean = false;
  private page!: number;

  constructor(private datePipe: DatePipe) {}

  setSavedSearchText(value: String) {
    this.savedSearchText = value;
  }

  getSavedSearchText(): String {
    return this.savedSearchText;
  }

  setSavedSatus(value: String) {
    this.savedSatus = value;
  }

  getSavedSatus(): String {
    return this.savedSatus;
  }

  setIsUpdate(value: boolean) {
    this.isUpdate = value;
  }

  getIsUpdate(): boolean {
    return this.isUpdate;
  }

  formatDate(date: Date): string | null {
    return this.datePipe.transform(date, 'dd.MM.yyyy');
  }

  setPage(value: number) {
    this.page = value;
  }

  getPage(): number {
    return this.page;
  }
}
