import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ApplicationRoutingModule } from './application-routing.module';
import { ApplicationComponent } from './application.component';
import { HrDashboardComponent } from './hr-dashboard/hr-dashboard.component';
import { CreateNewUserComponent } from './create-new-user/create-new-user.component';
import { ProfileComponent } from './profile/profile.component';

import { ProjectDashboardComponent } from './project-dashboard/project-dashboard.component';
import { ProjectComponent } from './project/project.component';

import { ReactiveFormsModule } from '@angular/forms';
import { TabsComponent } from './tabs/tabs.component';
import { MatTabsModule } from '@angular/material/tabs';
import { CreateNewEmployeeComponent } from './create-new-employee/create-new-employee.component';
import { AddTeamMembersComponent } from './add-team-members/add-team-members.component';
import { NewRoleComponent } from './new-role/new-role.component';
import { NewTeamComponent } from './new-team/new-team.component';


=====


@NgModule({
  declarations: [
    ApplicationComponent,
    HrDashboardComponent,
    CreateNewUserComponent,
    ProfileComponent,

    ProjectDashboardComponent,
    ProjectComponent
=======
    TabsComponent,
    CreateNewEmployeeComponent,
    AddTeamMembersComponent,
    NewRoleComponent,
    NewTeamComponent

  ],
  imports: [
    CommonModule,
    ApplicationRoutingModule,
    ReactiveFormsModule,
    MatTabsModule,
    CommonModule
  ]
})
export class ApplicationModule { }
