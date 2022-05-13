import { DatePipe } from "@angular/common";

export class ProjectModel {
    Id !: number;
    projectName !: string;
    projectDescription !: string
    PlannedStartDate !: DatePipe
    PlannedEndDate !: DatePipe
}