import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  constructor() { }

  passingDataToLocal(param: string, value: object) {
    localStorage.setItem(param, JSON.stringify(value));
  }

  getDataFromLocal(param: string) {
    let data: any = localStorage.getItem(param);
    return localStorage.getItem(param)? JSON.parse(data) : null;
  }

  removeDataFromLocal(param: string) {
    localStorage.removeItem(param);
  }

}
