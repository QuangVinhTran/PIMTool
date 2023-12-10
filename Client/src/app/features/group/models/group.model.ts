import { Employee } from '../../employee/models/employee.model';

export interface Group {
  id: number;
  version: number;
  groupLeaderId: number;
  employees: Employee[];
}
