import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  public baseApiUrl:string = "https://localhost:7232/Role/"
  constructor(private http: HttpClient) { }

  getAllRoles() {
    return this.http.get<any>(`${this.baseApiUrl}getAllRoles`)
  }
  addRole(roleObj: any) {
    return this.http.post<any>(`${this.baseApiUrl}addRole`,roleObj)
  }
  updateRole(roleObj: any) {
    return this.http.put<any>(`${this.baseApiUrl}updateRole`, roleObj);
  }
  removeRole(id: number) {
    return this.http.delete<any>(`${this.baseApiUrl}${id}`)
  }
}
