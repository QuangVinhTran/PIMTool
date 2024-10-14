import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {LocalStorageService} from "./local-storage.service";
import {map} from "rxjs/operators"
import {Observable, Subscription} from "rxjs";
import {Authentication} from "../model/authentication";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  isError: boolean = true;
  errorMessage: string = "";
  constructor(private http: HttpClient,
              private localStorageService: LocalStorageService) { }
  authentication(value: object): Observable<any> {
    return this.http.post("http://localhost:5150/auth/login", value, {observe: "response"});
    //  this.http
    //   .post("http://localhost:5150/auth/login", value, {observe: "response"}).subscribe((response: any) => {
    //     // this.localStorageService.bindingDataToLocal("token", res);
    //     return response;
    // }, error => {
    //    return "Wrong username or password";
    // })
  }

}
