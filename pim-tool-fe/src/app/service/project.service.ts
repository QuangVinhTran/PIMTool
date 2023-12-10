import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Project, ProjectMembers } from '../model/project';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private projectUrl: string = 'https://localhost:7099/api/Project';

  constructor(private http: HttpClient) {}

  public addProject(ProjectMembers: ProjectMembers): Observable<ProjectMembers> {
    return this.http.post<ProjectMembers>(`${this.projectUrl}`, ProjectMembers);
  }

  public getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.projectUrl}`);
  }

  public searchProjects(
    searchText: String,
    status: number
  ): Observable<Project[]> {
    return this.http.get<Project[]>(
      `${this.projectUrl}/search?searchText=${searchText}&status=${status}`
    );
  }

  public searchProjectsWithPagination(
    searchText: String,
    status: number,
    limit: number,
    skip: number
  ): Observable<Project[]> {
    return this.http.get<Project[]>(
      `${this.projectUrl}/search-with-pagination?searchText=${searchText}&status=${status}&limit=${limit}&skip=${skip}`
    );
  }

  public deleteProject(projectId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.projectUrl}?id=${projectId}`
    );
  }

  public getProjectByNumber(number: number): Observable<any> {
    return this.http.get<any>(`${this.projectUrl}/${number}`);
  }

  public updateProject(ProjectMembers: ProjectMembers): Observable<Project> {
    return this.http.put<Project>(`${this.projectUrl}?id=${ProjectMembers.ProjectDto.id}`, ProjectMembers);
  }

  public getProjectsPagination(limit: number, skip: number): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.projectUrl}/pagination?limit=${limit}&skip=${skip}`);
  }

  public getProjectsCount(): Observable<number> {
    return this.http.get<number>(`${this.projectUrl}/count`);
  }
}
