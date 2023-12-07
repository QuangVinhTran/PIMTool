import { Injectable } from '@angular/core';
import BASE_URL, {EndPoints} from "../../data/apiInfo";
import {SearchCriteria, SortInfo} from "../models/filter.models";
import {Store} from "@ngrx/store";
import {AdvancedFilterState} from "../store/advanced-filter/advancedFilter.reducers";
import {HttpClient} from "@angular/common/http";
import {catchError, Observable, of} from "rxjs";
import {getLocalAccessToken} from "../utils/localStorage.util";
import {Project} from "../models/project/project.models";
import {map} from "rxjs/operators";
import {selectFilterProperties, selectFilterStatus} from "../store/advanced-filter/advancedFilter.selectors";
import {AppState} from "../store/app.state";
import { ExportFileRequest } from 'src/app/modules/project/components/file-export/file-export.component';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  isAdvancedSearch = false

  constructor(private store: Store<AppState>, private http: HttpClient) {
    store.select(selectFilterStatus).subscribe(value => this.isAdvancedSearch = value)
  }

  apiResponse: ApiResponse = {
    isSuccess: false,
    messages: [],
    data: null
  }

  loadProjects(pageIndex: number, pageSize: number, searchCriteria: SearchCriteria, sortInfo: SortInfo): Observable<ApiResponse> {
    let payLoad = {
      pageSize,
      pageIndex: Math.max(pageIndex, 1),
      searchCriteria: searchCriteria,
      sortByInfos: sortInfo.fieldName ? [sortInfo] : []
    }
    let advancedFilterPayload;
    this.isAdvancedSearch && this.store.select(selectFilterProperties)
      .subscribe(value => advancedFilterPayload = {...payLoad, advancedFilter: value})
    const finalPayload = this.isAdvancedSearch ? advancedFilterPayload : payLoad;
    return this.http.post<ApiResponse>(
      `${BASE_URL}/${EndPoints.PROJECTS}`,
      finalPayload,
    ).pipe(
      catchError((err, caught) => {
        const errorResponse: ApiResponse = {
          isSuccess: false,
          messages: [],
          data: null,
        }
        return of(errorResponse)
      })
    )
  }

  getSingleProject(id: string): Observable<Project> {
    return this.http.post<ApiResponse>(
      `${BASE_URL}/${EndPoints.PROJECTS}/${id}`,
      null,
    ).pipe(
      map(value => value.data)
    )
  }

  updateProject(projectId: string, projectInfo: any, projectVersion: number): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(
      `${BASE_URL}/${EndPoints.UPDATE_PROJECT}/${projectId}`,
      {...projectInfo, version: projectVersion},
      {
        headers: {
          'UpdaterId': '295189a8-e4df-4b41-fd14-08dbdbacb07b'
        }
      },
    )
  }

  createProject(projectInfo: any): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(
      `${BASE_URL}/${EndPoints.CREATE_PROJECT}`,
      projectInfo,
    )
  }

  validateProjectNumber(projectNumber: number) {
    return this.http.post<ApiResponse>(
      `${BASE_URL}/${EndPoints.VALIDATE_PROJECT_NUMBER}/${projectNumber}`, null
    ).pipe(
      map(response => response.data)
    )
  }

  deleteProject(id: string) {
    this.http.delete(
      `${BASE_URL}/${EndPoints.DELETE_PROJECT}/${id}`,
    ).subscribe()
  }

  deleteMultipleProjects(projects: string[]): void {
    this.http.post<ApiResponse>(
      `${BASE_URL}/${EndPoints.DELETE_PROJECT}`,
      {
        projectIds: projects
      },
    ).subscribe()
  }

  importProjectsFromFile(file: File): Observable<ApiResponse> {
    const formData = new FormData()
    formData.append('file', file)

    return this.http.post<ApiResponse>(
      `${BASE_URL}/${EndPoints.IMPORT_PROJECTS}`,
      formData,{
        responseType: 'blob' as 'json'
      }
    )
  }

  exportProjectsToFile(request: ExportFileRequest): Observable<any> {
    return this.http.post<any>(
      `${BASE_URL}/${EndPoints.EXPORT_PROJECTS}`, 
      request,
      {
        responseType: 'blob' as 'json'
      }
    )
  }
}

export interface ApiResponse {
  isSuccess: boolean
  messages: ApiMessage[]
  data: any
}

export interface ApiMessage {
  content: string
  type: number
}
