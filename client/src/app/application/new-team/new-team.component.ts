import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TeamModel } from 'src/app/shared/models/team.model';
import { TeamService } from 'src/app/shared/services/team.service';

@Component({
  selector: 'app-new-team',
  templateUrl: './new-team.component.html',
  styleUrls: ['./new-team.component.scss']
})
export class NewTeamComponent implements OnInit {
  public teamForm !: FormGroup;
  public teamObj = new TeamModel();

  constructor(
    private formBuilder: FormBuilder,
    private teamServie: TeamService ) { }

  ngOnInit(): void {
    this.teamForm = this.formBuilder.group({
      teamName: [''],
    })
  }

  addNewTeam() {
    this.teamObj.teamName = this.teamForm.value.teamName;
    this.teamServie.addTeam(this.teamObj)
      .subscribe({
      next: (res) => {
          alert(res.message);
          this.teamForm.reset();
        },
        error: (err) => {
          console.log(err);
        }
      })
  }
}
