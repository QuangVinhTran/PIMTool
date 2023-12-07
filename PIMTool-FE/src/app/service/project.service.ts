import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Project} from "../model/project";
import {Response} from "../model/response";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  constructor(private http: HttpClient,
              private router: Router) {}

  getProjects(token: string | null) {
    console.log(token);
    return this.http.get<any>("http://localhost:5150/api/v1/Project/get-all", {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + token
      })
    });
  }

}
