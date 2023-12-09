import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Group } from '../models/Group';

@Injectable({
  providedIn: 'root',
})
export class GroupService {
  private apiGet = 'https://localhost:5001/api/Group';
  private apiGetGroupByIdUrl = 'https://localhost:5001/api/Group';

  constructor(private http: HttpClient) {}

  getGroup(): Observable<Group[]> {
    return this.http.get<Group[]>(this.apiGet);
  }

  getGroupById(id: any): Observable<Group> {
    const apiGetGroupByIdEndpoint = `${this.apiGetGroupByIdUrl}/${id}`;
    return this.http.get<Group>(apiGetGroupByIdEndpoint);
  }
}
