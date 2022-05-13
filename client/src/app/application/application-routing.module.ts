import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApplicationComponent } from './application.component';
import { CreateNewUserComponent } from './create-new-user/create-new-user.component';
import { HrDashboardComponent } from './hr-dashboard/hr-dashboard.component';
import { ProfileComponent } from './profile/profile.component';
import { ProjectDashboardComponent } from './project-dashboard/project-dashboard.component';
import { ProjectComponent } from './project/project.component';

const routes: Routes = [
  {
    path: '', component: ApplicationComponent,
    children: [
      { path: '', redirectTo:'hrDashboard', pathMatch:'full'},
      { path: 'hrDashboard', component: HrDashboardComponent},
      { path: 'createNewUser', component: CreateNewUserComponent },
      { path: 'profile/:id', component: ProfileComponent},
      { path: 'projectDashboard', component: ProjectDashboardComponent},
      { path: 'project/:id', component: ProjectComponent}
      
  ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ApplicationRoutingModule { }
