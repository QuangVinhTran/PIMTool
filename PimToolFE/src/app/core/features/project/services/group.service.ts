import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Group} from "../models/group.model";
import {environment} from "../../../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(private http:HttpClient ) {}

  getAllGroups(): Observable<Group[]> {
    return this.http.get<Group[]>(`${environment.apiBaseUrl}/Group`)
  }
}
