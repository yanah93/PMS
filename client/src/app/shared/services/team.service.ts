import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TeamService {

 
  public baseApiUrl:string = "https://localhost:7232/Team/"
  constructor(private http: HttpClient) { }

  getAllTeams() {
    return this.http.get<any>(`${this.baseApiUrl}getAllTeams`)
  }
  addTeam(teamObj: any) {
    return this.http.post<any>(`${this.baseApiUrl}addTeam`,teamObj)
  }
  editTeam(teamObj: any) {
    return this.http.put<any>(`${this.baseApiUrl}editTeam`, teamObj);
  }
  removeRole(id: number) {
    return this.http.delete<any>(`${this.baseApiUrl}${id}`)
  }
}
