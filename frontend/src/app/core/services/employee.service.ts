import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import BASE_URL, {EndPoints} from "../../data/apiInfo";
import {ApiResponse} from "./project.service";
import {map} from "rxjs/operators";
import {getLocalAccessToken} from "../utils/localStorage.util";

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  constructor(
    private http: HttpClient
  ) { }

  getEmployees() {
    return this.http.get<ApiResponse>(`${BASE_URL}/${EndPoints.EMPLOYEES_ALL}`)
      .pipe(map(response => response.data))
  }
}
