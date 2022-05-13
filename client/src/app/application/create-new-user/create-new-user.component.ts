import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UserModel } from 'src/app/shared/models/user.model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-create-new-user',
  templateUrl: './create-new-user.component.html',
  styleUrls: ['./create-new-user.component.scss']
})
export class CreateNewUserComponent implements OnInit {

  public userForm !: FormGroup;
  public userObj = new UserModel();
  public isPM: boolean = false;

  constructor(public auth: AuthService,
    private formBuilder: FormBuilder,
  private userService : UserService) { }

  ngOnInit(): void {
    this.userForm = this.formBuilder.group({
      Email: [''],
      UserName: [''],
      FirstName: [''],
      LastName: [''],
      isProjectManager:['']
    })
  }
  clickEvent() {
    this.isPM = !this.isPM
    console.log(this.isPM);
    
  }

  addNewUser() {
    this.userObj.email = this.userForm.value.Email;
    this.userObj.username = this.userForm.value.UserName;
    this.userObj.firstName = this.userForm.value.FirstName;
    this.userObj.lastName = this.userForm.value.LastName;
    this.userObj.isProjectManager = this.userForm.value.isProjectManager;
    this.userService.adduser(this.userObj)
      .subscribe({
      next: (res) => {
          alert(res.message);
          this.userForm.reset()
        },
        error: (err) => {
          console.log(err);
        }
      })
  }

}
