import { Employee } from './employee';

export interface Group {
  id: number;
  groupLeaderId: number;
  groupLeader: Employee;
}
