import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Project } from '../model/project';
import { Employee } from '../model/employee';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private empUrl: string = 'https://localhost:7099/api/Employee';

  constructor(private http: HttpClient) {}

  public getEmployees(){
    return this.http.get<any>(`${this.empUrl}`);
  }

  public searchEmployees(searchText: String){
    return this.http.get<any>(
      `${this.empUrl}/search?searchText=${searchText}`
    );
  }
}
