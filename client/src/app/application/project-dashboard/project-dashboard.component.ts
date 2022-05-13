import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProjectService } from 'src/app/shared/services/project.service';

@Component({
  selector: 'app-project-dashboard',
  templateUrl: './project-dashboard.component.html',
  styleUrls: ['./project-dashboard.component.scss'],
})
export class ProjectDashboardComponent implements OnInit {
  public projectList: any = [];

  constructor(private projectSvc: ProjectService, private router: Router) {}

  ngOnInit(): void {
    this.getAllProjects();
  }

  getAllProjects() {
    this.projectSvc.getAllProjects().subscribe({
      next: (res) => {
        console.log(res);
        this.projectList = res.result;
        console.log(res.result);
      },

      error: (err) => {
        console.log(err);
      },
    });
  }

  viewMore(id:number){
    this.router.navigate(['application/project', id])
    console.log(id)
  }

  deleteProject(id : number){
    this.projectSvc.deleteProject(id)
    .subscribe({
      next:(res)=>{
        alert(res.message);
        console.log(res)
        this.getAllProjects();
       
      },
      error:(err)=>{
        console.log(err)
      }
    })
  }
}
