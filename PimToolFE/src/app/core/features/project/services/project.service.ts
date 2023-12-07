import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Project} from "../models/project.model";
import {environment} from "../../../../../environments/environment";
import {ProjectParameters} from "../models/project-parameter.model";
import {CreateProjectModel} from "../models/create-project.model";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private http: HttpClient) { }

    getAllProjects(model: ProjectParameters) : Observable<Project[]> {
    return this.http.get<Project[]>(`${environment.apiBaseUrl}/Project?`
    + `PagingParameters.PageNumber=${model.pagingParameters.pageNumber}&`
    + `PagingParameters.PageSize=${model.pagingParameters.pageSize}`);
  }

    createAProject(model: CreateProjectModel) : Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/Project`, model);
    }
}
