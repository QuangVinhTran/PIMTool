import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { project } from '../models/project';
import { statusEnum } from '../enums/statusEnum';
@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private apiGetUrl = 'https://localhost:5001/api/Project';
  private apiSearchUrl = 'https://localhost:5001/api/projects/filter';
  private apiDeleteUrl = 'https://localhost:5001/api/Project';
  private apiDeleteProjectsUrl =
    'https://localhost:5001/api/projects/delete-projects';
  private apiIsProjectNumberExisted =
    'https://localhost:5001/api/projects/isExist';
  private apiGetProjectByIdUrl = 'https://localhost:5001/api/Project';

  private apiPostUrl = 'https://localhost:5001/api/Project';
  private apiUpdateProjectUrl = 'https://localhost:5001/api/Project';

  constructor(private http: HttpClient) {}

  getProject(
    pageIndex: number = 0,
    pageSize: number = 5
  ): Observable<project[]> {
    const apiGetProjectsEndpoint = `${this.apiGetUrl}?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<project[]>(apiGetProjectsEndpoint);
  }

  searchProject(
    searchTerm?: string,
    status?: statusEnum,
    pageIndex: number = 0,
    pageSize: number = 5
  ): Observable<project[]> {
    const statusParam = status !== undefined ? `&status=${status}` : '';
    const searchTermParam =
      searchTerm !== undefined ? `&searchTerm=${searchTerm}` : '';

    const apiSearchEndpoint = `${this.apiSearchUrl}?${searchTermParam}${statusParam}&pageIndex=${pageIndex}&pageSize=${pageSize}`;
    console.log(apiSearchEndpoint);
    return this.http.get<project[]>(apiSearchEndpoint);
  }

  isProjectNumberExisted(projectNumber: number): Observable<any> {
    const projectNumberParam =
      projectNumber !== undefined ? `&projectNumber=${projectNumber}` : '';

    const apiCheckIsProjectExisted = `${this.apiIsProjectNumberExisted}?${projectNumberParam}`;

    console.log(apiCheckIsProjectExisted);

    return this.http.get(apiCheckIsProjectExisted);
  }

  createProject(model: project): Observable<any> {
    return this.http.post<project>(this.apiPostUrl, model);
  }

  updateProject(projectId: number, model: project): Observable<any> {
    const apiUpdateProjectEndpoint = `${this.apiUpdateProjectUrl}/${projectId}`;

    return this.http.put<project>(apiUpdateProjectEndpoint, model);
  }

  deleteProject(projectId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiDeleteUrl}/${projectId}`);
  }

  deleteProjects(projectIds: number[]): Observable<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return this.http.request<any>('delete', this.apiDeleteProjectsUrl, {
      ...options,
      body: projectIds,
    });
  }

  getEmployeeInProject(projectId: number): Observable<any> {
    const apiGetEmployeeInProject = `https://localhost:5001/api/projects/${projectId}/employees`;

    return this.http.get<any>(apiGetEmployeeInProject);
  }

  getProjectById(projectId: any): Observable<project> {
    const apiGetProjectByIdEndpoint = `${this.apiGetUrl}/${projectId}`;

    return this.http.get<project>(apiGetProjectByIdEndpoint);
  }

  private searchState: {
    searchTerm: string;
    statusFilter: string;
    result: project[];
  } = { searchTerm: '', statusFilter: '', result: [] };

  setSearchState(state: {
    searchTerm: string;
    statusFilter: string;
    result: project[];
  }) {
    this.searchState = state;
  }

  getSearchState(): {
    searchTerm: string;
    statusFilter: string;
    result: project[];
  } {
    return this.searchState;
  }

  resetSearchState() {
    const emptyState: {
      searchTerm: string;
      statusFilter: string;
      result: project[];
    } = { searchTerm: '', statusFilter: '', result: [] };

    this.setSearchState(emptyState);
  }
}
