import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeModel } from 'src/app/shared/models/employee.model';
import { TeamMemberModel } from 'src/app/shared/models/teamMember.model';
import { UserModel } from 'src/app/shared/models/user.model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { EmployeeService } from 'src/app/shared/services/employee.service';
import { TeamMemberService } from 'src/app/shared/services/team-member.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  currentUserId !: number;
  userData !: UserModel;
  empData !: EmployeeModel;
  teamData :any = [];

  constructor(
    private activatedRouth: ActivatedRoute,
    public auth: AuthService,
    private userService: UserService,
    private employeeService: EmployeeService,
  private teamMemberService :TeamMemberService
  ) { }

  ngOnInit(): void {
    this.activatedRouth.params.subscribe(val => {
      this.currentUserId = val['id'];
      console.log(this.currentUserId);
    })
    this.getUserProfile();
    this.getEmployee();
  }

  getUserProfile() {
    this.userService.getUserById(Number(this.currentUserId))
      .subscribe((res: any) => {
        this.userData = res.result;
        console.log(this.userData);
    })
  }

  getEmployee() {
    this.employeeService.employeeById(Number(this.currentUserId))
      .subscribe((res: any) => {
        this.empData = res.results;
        console.log(this.empData);

        this.teamMemberService.byEmployeeId(Number(this.empData.id))
      .subscribe((res: any) => {
        this.teamData = res.result;
        console.log(this.teamData);
        
    })
  }
)
  }
}
