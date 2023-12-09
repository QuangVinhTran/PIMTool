import { HandleError } from './handleError.service';
import { Observable, catchError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Employee } from '../models/employee';
import { ResponseDto } from '../models/responseDto';
import { environment } from 'src/environment/environment ';
import { EmployeeDropdown } from '../models/employeeDropdown';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private url = 'employee';

  constructor(private http: HttpClient, private handleError : HandleError) {}

  public getAll(): Observable<ResponseDto> {
    return this.http
      .get<ResponseDto>(`${environment.urlApi}/${this.url}`).pipe(
        catchError(
          this.handleError.handleError
          ));
  }

  public employeeToEmployeeDropdown(list: Employee[]): EmployeeDropdown[] {
    let result: EmployeeDropdown[] = [];
    for(let a of list) {
      let id = a.id;
      if(id) {
        let employeeDropdown = new EmployeeDropdown(id,`${a.visa}: ${a.firstName} ${a.lastName}`);
        result.push(employeeDropdown);
      }
    }
    return result;
  }
  public employeeDropdownToEmployee(listEmployeeDropdown : EmployeeDropdown[], listEmployeeAll : Employee[]) : Employee[] {
    const result : Employee[] = [];
    for(let edd of listEmployeeDropdown) {
      for(let ea of listEmployeeAll) {
        if(edd.id == ea.id) {
          result.push(ea);
          break;
        }
      }
    }
    return result;
  }
}
