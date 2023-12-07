import { Pipe, PipeTransform } from '@angular/core';
import {Employee} from "../models/project/employee.model";

@Pipe({
  name: 'employeeFormat'
})
export class EmployeeFormatPipe implements PipeTransform {

  transform(value: Employee, ...args: unknown[]): unknown {
    return `${value.visa}: ${value.firstName} ${value.lastName}`;
  }

}
