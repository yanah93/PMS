import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TeamMemberService {

  public baseApiUrl:string = "https://localhost:7232/TeamMember/"
  constructor(private http: HttpClient) { }

  getAllTeamMembers() {
    return this.http.get<any>(`${this.baseApiUrl}getAllTeamMembers`)
  }
  byTeamId(id: number) {
    return this.http.get<any>(`${this.baseApiUrl}getTeamMembersByTeamId_${id}`)
  }
  byTeamName(name: string) {
    return this.http.get<any>(`${this.baseApiUrl}getTeamMembersByTeamName_${name}`)
  }
  byEmployeeId(id: number) {
    return this.http.get<any>(`${this.baseApiUrl}getTeamMembersByEmployeeId_${id}`)
  }
  addTeamMembers(teamObj: any) {
    return this.http.post<any>(`${this.baseApiUrl}addTeamMembersToTeam`,teamObj)
  }
  updateTeamMember(memberObj: any) {
    return this.http.put<any>(`${this.baseApiUrl}editMember`, memberObj);
  }
  removeMember(id: number) {
    return this.http.delete<any>(`${this.baseApiUrl}${id}`)
  }
}
