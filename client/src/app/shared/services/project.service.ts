import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  public baseApiUrl:string = "https://localhost:7232/api/Project/" 
  
  constructor(private http : HttpClient) { }

  addProject(formData : any){
    return this.http.post<any>(`${this.baseApiUrl}add`,formData);
  }
  getAllProjects(){
    return this.http.get<any>(this.baseApiUrl);
  }
  updateProject(empObj : any){
    return this.http.put<any>(`${this.baseApiUrl}update`,empObj);
  }
  deleteProject(id:number){
    return this.http.delete<any>(`${this.baseApiUrl}${id}`)
  }
  getProjectbyId(id:number){
    return this.http.get<any>(`${this.baseApiUrl}${id}`);
  }
}
