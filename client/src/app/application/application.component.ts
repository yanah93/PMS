import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.scss']
})
export class ApplicationComponent implements OnInit {

  public loggedInUser: string = "";
  status: boolean = false;
  currentRole: string = "";
  employeeList: any = [];
  public author: any = [];
  user !: any;

  constructor(public auth:AuthService, private router:Router, private userService: UserService) { }

  ngOnInit(): void {
    this.getRole();
    this.getUser();
    this.getProfile();
  }

  clickEvent() {
    this.status = !this.status //toggle
  }
  getRole() {
    this.currentRole = this.auth.getUserRole();
  }
  seeDetails() {
    this.router.navigate(['application/profile/', Number(this.auth.getUserId())]);
  }

  getUser() {
    this.user = this.auth.getUserId();
    console.log(this.user);
    
  }
  getProfile() {
    this.userService.getUserById(this.user)
    .subscribe({
      next: (res) => {
        this.user = res.result;
        
        
        console.log(this.user);
        
    }
  })
  }
}
