import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../../../../environments/environment";
import {ProjectParameters} from "../models/project-parameter.model";
import {Project} from "../models/project";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private http: HttpClient) { }

    getAllProjects(model: ProjectParameters) : Observable<Project[]> {
    let query = `${environment.apiBaseUrl}/Project?`
        + `PagingParameters.PageNumber=${model.pagingParameters.pageNumber}&`
        + `PagingParameters.PageSize=${model.pagingParameters.pageSize}`;

    if(model.filterParameters?.trim() !== "" && model.filterParameters !== undefined)
      query += `&FilterParameters=${model.filterParameters}`;

    if(model.status !== "" && model.status !== undefined) {
      query += `&Status=${model.status}`;
    }

    return this.http.get<Project[]>(query);
  }

    getAProject(id: number) : Observable<Project> {
    return this.http.get<Project>(`${environment.apiBaseUrl}/Project/${id}`)
    }

    createAProject(model: Project) : Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/Project`, model);
    }

    checkProjectNumber(number: number) : Observable<Project> {
      return this.http.get<Project>(`${environment.apiBaseUrl}/Project/Number?number=${number}`);
    }

    checkMemberVisa(visaList: string) : Observable<string[]> {
    return this.http.get<string[]>(`${environment.apiBaseUrl}/Employee/Visa?visaList=${visaList}`);
    }

    editAProject(model: Project) : Observable<void> {
    return this.http.put<void>(`${environment.apiBaseUrl}/Project`, model);
    }

    deleteAProject(id: number) : Observable<void> {
    return this.http.delete<void>(`${environment.apiBaseUrl}/Project/${id}`)
    }

    deleteMultipleProjects(id: number[]) : Observable<void> {
    return this.http.delete<void>(`${environment.apiBaseUrl}/Project`, { body: id });
    }
}
