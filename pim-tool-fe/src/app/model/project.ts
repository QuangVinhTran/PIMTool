import { Group } from './group';

export enum Status {
  NEW = 0,
  PLA = 1,
  INP = 2,
  FIN = 3,
}

export interface Project {
  id: number;
  projectNumber: number;
  name: string;
  customer: string;
  group: Group;
  groupId: number;
  groupLeaderVisa: string;
  members: [];
  status: Status;
  startDate: Date;
  endDate: Date;
  version: number;
}

export interface ProjectMembers{
  ProjectDto: Project;
  ListEmpId: number[];
}
