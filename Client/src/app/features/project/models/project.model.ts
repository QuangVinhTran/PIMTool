export interface Project {
  projectNumber: number;
  name: string;
  customer: string;
  status: string;
  startDate: Date;
  endDate: Date;
  checked?: boolean;
}
