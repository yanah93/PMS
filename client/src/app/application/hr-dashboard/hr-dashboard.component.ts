import { Component, OnInit } from '@angular/core';
import { UserModel } from 'src/app/shared/models/user.model';
import { TeamService } from 'src/app/shared/services/team.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-hr-dashboard',
  templateUrl: './hr-dashboard.component.html',
  styleUrls: ['./hr-dashboard.component.scss']
})
export class HrDashboardComponent implements OnInit {

  userList: any = [];
  teamList: any = [];
  constructor(private userService: UserService,
  private teamService:TeamService) { }

  ngOnInit(): void {
    this.getAllUsers();
    this.getAllTeams();
  }

  getAllUsers() {
    this.userService.getAllUsers()
      .subscribe((res: any) => {
        this.userList = res.result;
        console.log(this.userList);
        
      })
  }
  getAllTeams() {
    this.teamService.getAllTeams()
      .subscribe((res: any) => {
        this.teamList = res.result;
        console.log(this.teamList);
        
    })
  }
}
