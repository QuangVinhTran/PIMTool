import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { CreateProjectRequest } from '../models/create-project.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Project } from '../models/project.model';
import { PageList } from '../models/page-list-model';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  constructor(private http: HttpClient) {}

  CreateProject(model: CreateProjectRequest): Observable<any> {
    return this.http.post<any>(`${environment.apiBaseUrl}/project`, model);
  }

  GetAllProject(pageNumber: number, pageSize: number): Observable<PageList> {
    return this.http.get<PageList>(
      `${environment.apiBaseUrl}/project?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }

  SearchProject(
    searchValue: string,
    status: string,
    pageNumber: number,
    pageSize: number
  ): Observable<PageList> {
    return this.http.get<PageList>(
      `${environment.apiBaseUrl}/project/search?searchValue=${searchValue}&status=${status}&pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }

  GetByProjectNumber(projectNumber: number): Observable<CreateProjectRequest> {
    return this.http.get<CreateProjectRequest>(
      `${environment.apiBaseUrl}/project/get-by-project-num/${projectNumber}`
    );
  }

  UpdateProject(
    projectNumber: number,
    model: CreateProjectRequest
  ): Observable<any> {
    return this.http.put<any>(
      `${environment.apiBaseUrl}/project/${projectNumber}`,
      model
    );
  }

  DeleteProject(projectIds: number[]): Observable<any> {
    let stringParams = projectIds
      .reduce((prev, cur) => {
        return (prev += `projectNumbers=${cur}&`);
      }, '')
      .slice(0, -1);
    return this.http.delete<any>(
      `${environment.apiBaseUrl}/project?${stringParams}`
    );
  }
}
