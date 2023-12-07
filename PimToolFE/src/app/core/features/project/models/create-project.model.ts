export interface CreateProjectModel{
  projectNumber?: string;
  name?: string;
  customer?: string;
  groupId?: number;
  employeeVisas?: string[];
  status: number;
  startDate?: Date;
  endDate?: Date;
}
