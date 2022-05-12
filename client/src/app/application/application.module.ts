import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ApplicationRoutingModule } from './application-routing.module';
import { ApplicationComponent } from './application.component';
import { HrDashboardComponent } from './hr-dashboard/hr-dashboard.component';
import { CreateNewUserComponent } from './create-new-user/create-new-user.component';
import { ProfileComponent } from './profile/profile.component';


@NgModule({
  declarations: [
    ApplicationComponent,
    HrDashboardComponent,
    CreateNewUserComponent,
    ProfileComponent
  ],
  imports: [
    CommonModule,
    ApplicationRoutingModule
  ]
})
export class ApplicationModule { }
