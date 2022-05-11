import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms' //Step 1
import { Router } from '@angular/router';
import { ResetModel } from '../shared/models/reset.model';
import { AuthService } from '../shared/services/auth.service';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public click: boolean = false;
  public loginForm !: FormGroup; //Step 2
  hidePassword: boolean = true;

  public resetPWForm !: FormGroup;
  public resetPWObj = new ResetModel();

  constructor(private formBuilder: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
  private router : Router ) { }//step 3 Injecting formbuilder

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({ //step 4 initialize the form
      email: ['', Validators.compose([Validators.required, Validators.email])],
      password: ['', Validators.required]
    });
    this.resetPWForm = this.formBuilder.group({
      email:[''],
    })

    localStorage.removeItem("token");

  }

  login() {
    if (this.loginForm.valid) {
      this.authService.loginUser(this.loginForm.value).subscribe({
        next: (res) => {
          alert(res.message);

          localStorage.setItem('token', res.token);
          if (this.authService.getUserRole() === 'False') {
            this.router.navigate(['blog/blogdashboard'])
          } else if (this.authService.getUserRole() === 'True') {
            this.router.navigate(['blog/dashboard/', Number(this.authService.getUserId())])
          }
          else {
            this.router.navigate(['blog/setting'])
          }
        },
        error: (err) => {
          alert("Email and password does not match!")
          console.log(err);
        }
      }); 
    
    } else {
        alert("Form not valid")
      }
  }

  resetPassword() {
    this.resetPWObj.email = this.resetPWForm.value.email;
    this.authService.resetPassword(this.resetPWObj)
      .subscribe({
        next: (res) => {
          alert(res.message);
          //this.login();
          this.resetPWForm.reset();
          document.getElementById("close-emp")?.click();
        },
        error: (err) => {
          alert("Invalid email");
          console.log(err);
        }
    })
  }
  
  toggleShow() {
    this.hidePassword = !this.hidePassword;
    console.log(this.hidePassword);
    
  }
  onClick() {
    this.click = !this.click;
    console.log(this.click);
    
  }
}
