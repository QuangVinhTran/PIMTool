import { Injectable } from '@angular/core';
import BASE_URL, {EndPoints} from "../../data/apiInfo";
import {HttpClient} from "@angular/common/http";
import {getLocalAccessToken} from "../utils/localStorage.util";
import {ApiResponse} from "./project.service";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(private http: HttpClient) { }

  getGroups(): Observable<any> {
    return this.http.get<ApiResponse>(
      `${BASE_URL}/${EndPoints.GROUPS_ALL}`
    ).pipe(
      map(value => value.data)
    )
  }
}
