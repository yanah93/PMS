import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { EmployeeModel } from 'src/app/shared/models/employee.model';
import { EmployeeService } from 'src/app/shared/services/employee.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-create-new-employee',
  templateUrl: './create-new-employee.component.html',
  styleUrls: ['./create-new-employee.component.scss']
})
export class CreateNewEmployeeComponent implements OnInit {


  public employeeForm !: FormGroup;
  public employeeObj = new EmployeeModel();
  userList: any = [];

  constructor(
    private formBuilder: FormBuilder,
    private employeeService: EmployeeService,
  private userService :UserService ) { }

  ngOnInit(): void {
    this.employeeForm = this.formBuilder.group({
      employeeeCode: [''],
      employeeName: [''],
      userAccountId: [''],
    })
    this.getAllUsers();
  }

  addNewEmp() {
    if (this.employeeForm.valid) {
      this.employeeObj.employeeeCode = this.employeeForm.value.employeeeCode;
    this.employeeObj.employeeName = this.employeeForm.value.employeeName;
    this.employeeObj.userAccountId = this.employeeForm.value.userAccountId;
      this.employeeService.addEmployee(this.employeeObj)
        .subscribe({
          next: (res) => {
            alert(res.message);
            this.employeeForm.reset();
          },
          error: (err) => {
            alert("Either Employee Code or Employee Name has been registered. Choose a different user to register.")
            console.log(err);
          }
        });
    } else {
      alert("Form not valid")
    }
    
  }

  getAllUsers() {
    this.userService.getAllUsers()
      .subscribe({
        next: (res) => {
          console.log(res);
          this.userList = res.result;
      }
    })
  }

}
