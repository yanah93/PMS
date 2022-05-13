import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PmModel } from 'src/app/shared/models/pm.model';
import { ProjectModel } from 'src/app/shared/models/project.model';
import { PmService } from 'src/app/shared/services/pm.service';
import { ProjectService } from 'src/app/shared/services/project.service';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {

  projectId !: number;
  projectData !: ProjectModel;
  pmId !: number;
  pmData !: PmModel;

  constructor(private projectSvc : ProjectService, 
    private activatedRoute : ActivatedRoute,
    private pmSvc : PmService) { }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe(val=>{
      this.projectId = val['id'];
      console.log(this.projectId);
    })



    this.getProject();
  }

  getProject(){
    this.projectSvc.getProjectbyId(Number(this.projectId))
    .subscribe((res:any)=>{
      this.projectData = res.result;
      console.log(res)
      console.log(this.projectData)
    })
  }

  getPM(){
    this.pmSvc.getPMbyId(Number(this.pmId))
    .subscribe((res:any) => {
      this.pmData = res.result;
      console.log(res)
      console.log(this.pmData)
    })
  }

}
