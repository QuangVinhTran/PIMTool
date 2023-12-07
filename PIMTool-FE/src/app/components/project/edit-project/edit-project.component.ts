import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Group } from 'src/app/models/Group';
import { employee } from 'src/app/models/employee';
import { EmployeeService } from 'src/app/services/employee.service';
import { GroupService } from 'src/app/services/group.service';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-edit-project',
  templateUrl: './edit-project.component.html',
  styleUrls: ['./edit-project.component.scss'],
})
export class EditProjectComponent implements OnInit {
  projectForm: FormGroup;
  groups: Group[] | null = null;
  group: Group | null = null;
  error: string | null = null;

  projectId!: any;
  project: any;

  suggestedEmployees: employee[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private groupService: GroupService,
    private projectService: ProjectService,
    private employeeService: EmployeeService,
    private datePipe: DatePipe
  ) {
    this.projectForm = this.fb.group({
      projectNumber: [
        null,
        [Validators.required, Validators.min(1000), Validators.max(9999)],
      ],
      name: [null, [Validators.required, Validators.maxLength(50)]],
      customer: [null, [Validators.required, Validators.maxLength(50)]],
      status: [0, Validators.required],
      startDate: [null, Validators.required],
      endDate: [null],
      groupId: [null, Validators.required],
      selectedEmployeeId: [null],
    });
  }

  ngOnInit(): void {
    this.projectId = this.route.snapshot.paramMap.get('id');
    console.log(this.projectId);

    this.getAllGroup();
    this.getProjectById(this.projectId);

    this.getEmployeeInProject(this.projectId);
  }

  getProjectById(projectId: any) {
    this.projectService.getProjectById(projectId).subscribe(
      (data: any) => {
        this.project = data.data;

        // After fetching data, update form
        const formattedStartDate = this.datePipe.transform(
          this.project.startDate,
          'yyyy-MM-dd'
        );
        const formattedEndDate = this.datePipe.transform(
          this.project.endDate,
          'yyyy-MM-dd'
        );

        this.projectForm.patchValue({
          projectNumber: this.project.projectNumber,
          name: this.project.name,
          customer: this.project.customer,
          status: this.project.status,
          startDate: formattedStartDate,
          endDate: formattedEndDate,
          groupId: this.project.groupId,
          //selectedEmployeeId: this.project.selectedEmployeeId,
        });

        this.getEmployeeInProject(projectId);

        console.log(this.project.groupId, 'group id day thay');
      },
      (error: any) => {
        console.log('Error: ', error);
        this.error = 'An error occurred while fetching project data by Id';
      }
    );
  }

  getAllGroup() {
    this.groupService.getGroup().subscribe(
      (data: any) => {
        this.groups = data.data;
      },
      (error: any) => {
        console.error('Error:', error);
        this.error = 'An error occurred while fetching group data.';
      }
    );
  }

  getEmployeeInProject(projectId: number) {
    this.projectService.getEmployeeInProject(projectId).subscribe(
      (data: any) => {
        //this.projectForm.get('selectedEmployeeId')?.setValue = data.data;
        console.log(data.data);
        this.projectForm.get('selectedEmployeeId')?.patchValue(data.data);
      },
      (error) => {
        console.log('Error: ', error);
        this.error = 'An error occurred while fetching employee in project';
      }
    );
  }

  private editProject() {
    // convert status has type string to int
    const convertStatus = Number(this.projectForm.get('status')?.value);
    this.projectForm.get('status')?.setValue(convertStatus);

    const selectedEmployee = this.projectForm.get('selectedEmployeeId')
      ?.value as employee[];

    const employeeIds: any[] = selectedEmployee.map((emp: any) => emp.id);

    console.log(employeeIds);

    this.projectForm.get('selectedEmployeeId')?.setValue(employeeIds);

    //const employeeIds: number[] = selectedEmployee?.map((emp: any) => emp.id);

    return this.projectService.updateProject(
      this.projectId,
      this.projectForm.value
    );
  }

  SearchEmployee(event: any) {
    const query = event.query;

    console.log(query);

    this.employeeService.searchEmployeesByVisa(query).subscribe(
      (data: employee[] | any) => {
        this.suggestedEmployees = data;
      },
      (error) => {
        console.error('Error fetching suggested employees:', error);
      }
    );
  }

  onSubmit(): void {
    if (this.projectForm.valid) {
      this.editProject().subscribe({
        next: () => {
          this.router.navigate([`/`]);
        },
      });
    } else {
      console.log('Error when update project');
    }
  }
  navigateToProjectList() {
    this.router.navigate([`/`]);
  }
}
