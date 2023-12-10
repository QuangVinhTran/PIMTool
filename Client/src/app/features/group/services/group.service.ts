import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Group } from '../models/group.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class GroupService {
  constructor(private http: HttpClient) {}

  getAllGroup(): Observable<Group[]> {
    return this.http.get<Group[]>(`${environment.apiBaseUrl}/group/get-all`);
  }
}
