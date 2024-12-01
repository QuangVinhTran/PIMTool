import { statusEnum } from '../enums/statusEnum';

export interface project {
  id: number;
  projectNumber: number;
  name: string;
  status: statusEnum;
  customer: string;
  startDate: Date;
  endDate: Date;
  selectedEmployeeId: number[];
}
