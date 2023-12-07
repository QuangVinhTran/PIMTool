import { EmployeeService } from './../../../services/employee.service';
import { ProjectService } from 'src/app/services/project.service';
import { Component, OnInit } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';

import {
  FormGroup,
  FormBuilder,
  Validators,
  AbstractControl,
} from '@angular/forms';
import {
  Subscription,
  debounceTime,
  distinctUntilChanged,
  of,
  switchMap,
} from 'rxjs';
import { Group } from 'src/app/models/Group';
import { GroupService } from 'src/app/services/group.service';
import { employee } from 'src/app/models/employee';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.scss'],
})
export class CreateProjectComponent implements OnInit {
  projectForm: FormGroup;
  groups: Group[] | null = null;
  employee: employee[] | null = null;
  suggestedEmployees: employee[] = [];

  // region handle showing validation error message
  dismissible = true;
  defaultAlerts: any[] = [
    {
      type: 'danger',
      msg: `Please enter all the mandatory fields (*)`,
    },
  ];
  alerts = this.defaultAlerts;

  error: string | null = null;

  private projectNumberSubscription: Subscription | undefined;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private groupService: GroupService,
    private projectService: ProjectService,
    private employeeService: EmployeeService,
    private primengConfig: PrimeNGConfig
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

    // Không thêm validator khi tạo form
    const endDateControl = this.projectForm.get('endDate');
    endDateControl?.setValidators(this.endDateValidator.bind(this));
  }

  ngOnInit() {
    this.getAllGroup();

    this.primengConfig.ripple = true;

    const projectNumberControl = this.projectForm.get('projectNumber');

    this.projectNumberSubscription = projectNumberControl?.valueChanges
      .pipe(
        debounceTime(300), // Chờ 300ms sau khi người dùng nhập xong trước khi gọi API
        distinctUntilChanged(), // Chỉ gọi API khi giá trị thay đổi
        switchMap((value: number) => {
          console.log(typeof value);
          if (value && value.toString().length == 4) {
            // Gọi API chỉ khi giá trị nhập vào đủ lớn (ví dụ: 3 ký tự)
            console.log('value length is greater than 4');
            console.log(value);
            return this.projectService.isProjectNumberExisted(value);
          } else {
            // Nếu giá trị nhập vào không đủ lớn, trả về Observable of false
            console.log('error not have value to call api');
            console.log(value);
            return of(false);
          }
        })
      )
      .subscribe(
        (data) => {
          if (data) {
            projectNumberControl.setErrors({ projectExists: true });
          }
        },
        (error) => {
          console.log(error);
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
    console.log(this.projectForm.value);
    if (this.projectForm.invalid) {
      console.log(this.projectForm.value);
      return;
    }

    this.saveProject().subscribe({
      next: () => {
        this.router.navigate([`/`]);
      },
    });
  }

  private saveProject() {
    // convert status has type string to int
    const convertStatus = Number(this.projectForm.get('status')?.value);
    this.projectForm.get('status')?.setValue(convertStatus);

    const selectedEmployee = this.projectForm.get('selectedEmployeeId')
      ?.value as employee[];

    // const employeeIds: any[] = selectedEmployee.map((emp: any) => emp.id);
    let employeeIds: any[] = [];
    if (selectedEmployee) {
      // Lặp qua từng phần tử để lấy id và thêm vào mảng employeeIds
      for (const emp of selectedEmployee) {
        employeeIds.push(emp.id);
      }
    }

    console.log(employeeIds);

    this.projectForm.get('selectedEmployeeId')?.setValue(employeeIds);

    return this.projectService.createProject(this.projectForm.value);
  }

  onCancel(): void {
    // if (this.projectForm) {
    //   this.projectForm.reset();
    // }
    this.router.navigate([`/`]);
  }

  isFormInvalid(): boolean {
    return this.projectForm.invalid && this.projectForm.touched;
  }

  get startDate() {
    return this.projectForm.get('startDate');
  }

  endDateValidator(
    control: AbstractControl
  ): { [key: string]: boolean } | null {
    const startDateControl = this.projectForm.get('startDate');
    if (!startDateControl) {
      return null;
    }

    const startDateValue = startDateControl.value;
    const endDateValue = control.value;

    // Kiểm tra chỉ khi có giá trị
    if (startDateValue && endDateValue && startDateValue >= endDateValue) {
      return { invalidDateRange: true };
    }

    return null;
  }

  reset(): void {
    this.alerts = this.defaultAlerts;
  }

  onClosed(dismissedAlert: any): void {
    this.alerts = this.alerts.filter((alert) => alert !== dismissedAlert);
  }

  ngOnDestroy() {
    // Đảm bảo huỷ subscription khi component bị hủy
    if (this.projectNumberSubscription) {
      this.projectNumberSubscription.unsubscribe();
    }
  }
}
