export interface ProjectModel {
  projectNumber?: number;
  name?: string;
  customer?: string;
  groupId?: number;
  employeeVisas?: string[];
  status: number;
  startDate?: Date;
  endDate?: Date;
}
