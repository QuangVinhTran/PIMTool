import { Timestamp } from "rxjs";
import { Employee } from "./employee";

export class Project {
  id?: number;
  groupId?: number;
  projectNumber?: number;
  name = "";
  customer= "";
  employees: Employee[] = [];
  status = "";
  startDate = new Date();
  endDate? : any;
  version = '';
  // constructor(
  //   id: number,
  //   groupId: number,
  //   projectNumber: number,
  //   name: string,
  //   customer: string,
  //   status: string,
  //   startDate: Date,
  //   endDate: Date
  // ) {
  //   this.id = id;
  //   this.groupId = groupId;
  //   this.projectNumber = projectNumber;
  //   this.name = name;
  //   this.customer = customer;
  //   this.status = status;
  //   this.startDate = startDate;
  //   this.endDate = endDate;
  // }
}
