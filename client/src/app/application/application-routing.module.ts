import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApplicationComponent } from './application.component';
import { CreateNewUserComponent } from './create-new-user/create-new-user.component';
import { HrDashboardComponent } from './hr-dashboard/hr-dashboard.component';
import { ProfileComponent } from './profile/profile.component';

const routes: Routes = [
  {
    path: '', component: ApplicationComponent,
    children: [
      { path: '', redirectTo:'hrDashboard', pathMatch:'full'},
      { path: 'hrDashboard', component: HrDashboardComponent},
      { path: 'createNewUser', component: CreateNewUserComponent },
      { path: 'profile/:id', component: ProfileComponent},
      
  ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ApplicationRoutingModule { }
