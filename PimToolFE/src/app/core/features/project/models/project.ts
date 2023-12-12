export interface Project {
  id?: number
  projectNumber?: number;
  name?: string;
  customer?: string;
  groupId?: number;
  employeeVisas?: string[];
  status?: string;
  startDate?: string | null;
  endDate?: string | null;
  totalPage?: number;
  version?: Uint8Array;
}
