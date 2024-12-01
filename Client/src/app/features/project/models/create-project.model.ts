export interface CreateProjectRequest {
  projectNumber: number;
  name: string;
  customer: string;
  groupId: number;
  members: number[];
  status: string;
  startDate: string;
  endDate: string;
  version?: string;
}
