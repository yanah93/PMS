import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { RoleModel } from 'src/app/shared/models/role.model';
import { RoleService } from 'src/app/shared/services/role.service';


@Component({
  selector: 'app-new-role',
  templateUrl: './new-role.component.html',
  styleUrls: ['./new-role.component.scss']
})
export class NewRoleComponent implements OnInit {

  public roleForm !: FormGroup;
  public roleObj = new RoleModel();


  constructor(
    private formBuilder: FormBuilder,
    private roleServie: RoleService ) { }

  ngOnInit(): void {
    this.roleForm = this.formBuilder.group({
      roleName: [''],
    })
  }

  addNewRole() {
    this.roleObj.roleName = this.roleForm.value.roleName;
    this.roleServie.addRole(this.roleObj)
      .subscribe({
      next: (res) => {
          alert(res.message);
          this.roleForm.reset();
        },
        error: (err) => {
          console.log(err);
        }
      })
  }
}
