import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public baseApiUrl:string = "https://localhost:7232/Auth"
  constructor(private http: HttpClient, private router: Router) {
    this.getLoggedInUser();
    this.getUserId();
    this.getUserEmail();
    this.getUserRole();
   }

  loginUser(loginObj:any) {
    return this.http.post<any>(`${this.baseApiUrl}`, loginObj)
  }
  updatePassword(loginObj:any) {
    return this.http.put<any>(`${this.baseApiUrl}updatePassword`,  loginObj)
  }
  resetPassword(loginObj:any) {
    return this.http.put<any>(`${this.baseApiUrl}resetPassword`, loginObj)
  }

  isUserLoggedIn(): boolean
  {
    return !!localStorage.getItem("token");
  }
  private getToken() {
    return localStorage.getItem("token")!;
  }

  getLoggedInUser() {
    if (this.isUserLoggedIn()) {
      let token = this.getToken();
      let decodedJWT = JSON.parse(window.atob(token?.split('.')[1]));
      return decodedJWT.Name ? decodedJWT.Name : '';
    }
  }
  getUserEmail() {
    if (this.isUserLoggedIn()) {
      let token = this.getToken();
      let decodedJWT = JSON.parse(window.atob(token?.split('.')[1]));
      //console.log(decodedJWT.Name);
      return decodedJWT.Email ? decodedJWT.Email : '';
    }
  }
  getUserId() {
    if (this.isUserLoggedIn()) {
      let token = this.getToken();
      let decodedJWT = JSON.parse(window.atob(token?.split('.')[1]));
      return decodedJWT.UserId;
    }
  }
  getUserRole() {
    if (this.isUserLoggedIn()) {
      let token = this.getToken();
      let decodedJWT = JSON.parse(window.atob(token?.split('.')[1]));
      return decodedJWT.ProjectManager;
    }
  }
}
