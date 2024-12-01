import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { employee } from '../models/employee';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private apiGetAllEmployee = 'https://localhost:5001/api/Employee';
  private apiSearchEmployeeByVisa =
    'https://localhost:5001/api/Employee/get-employees-by-visa';

  constructor(private http: HttpClient) {}

  getEmployees(): Observable<employee[]> {
    return this.http.get<employee[]>(this.apiGetAllEmployee);
  }

  searchEmployeesByVisa(visa: string): Observable<employee[]> {
    const visaParam = visa !== undefined ? `${visa}` : '';
    const endpoint = `${this.apiSearchEmployeeByVisa}/${visaParam}`;

    return this.http.get<employee[]>(endpoint);
  }
}
