import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TeamMemberModel } from 'src/app/shared/models/teamMember.model';
import { EmployeeService } from 'src/app/shared/services/employee.service';
import { RoleService } from 'src/app/shared/services/role.service';
import { TeamMemberService } from 'src/app/shared/services/team-member.service';
import { TeamService } from 'src/app/shared/services/team.service';


@Component({
  selector: 'app-add-team-members',
  templateUrl: './add-team-members.component.html',
  styleUrls: ['./add-team-members.component.scss']
})
export class AddTeamMembersComponent implements OnInit {

  public teamMemberForm !: FormGroup;
  public teamMemberObj = new TeamMemberModel();

  teamList: any = [];
  roleList: any = [];
  employeeList: any = [];

  constructor(
    private formBuilder: FormBuilder,
    private employeeService: EmployeeService,
    private teamService: TeamService,
    private teamMemberService: TeamMemberService,
  private roleService: RoleService
  ) { }

  ngOnInit(): void {
    this.teamMemberForm = this.formBuilder.group({
      teamId: [''],
      roleId: [''],
      employeeId: [''],
    })
    this.getAllTeams();
    this.getAllRoles();
    this.getAllEmp();
  }
 

  addNewteamMember() {
    this.teamMemberObj.teamId = this.teamMemberForm.value.teamId;
    this.teamMemberObj.roleId = this.teamMemberForm.value.roleId;
    this.teamMemberObj.employeeId = this.teamMemberForm.value.employeeId;
    this.teamMemberService.addTeamMembers(this.teamMemberObj)
      .subscribe({
      next: (res) => {
          alert(res.message);
          this.teamMemberForm.reset()
        },
        error: (err) => {
          console.log(err);
        }
      })
  }

  getAllTeams() {
    this.teamService.getAllTeams()
      .subscribe({
        next: (res) => {
          console.log(res);
          this.teamList = res.result;
      }
    })
  }

  getAllRoles() {
    this.roleService.getAllRoles()
      .subscribe({
        next: (res) => {
          console.log(res);
          this.roleList = res.result
      }
    })
  }
  getAllEmp() {
    this.employeeService.getAllEmployee()
      .subscribe({
        next: (res) => {
          console.log(res.result);
          this.employeeList = res.result;
      }
    })
  }
}
