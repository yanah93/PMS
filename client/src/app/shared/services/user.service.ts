import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  public baseApiUrl:string = "https://localhost:7232/Users/"
  constructor(private http: HttpClient) { }

  getAllUsers() {
    return this.http.get<any>(`${this.baseApiUrl}getAllUsers`)
  }
  getUserById(id: number) {
    return this.http.get<any>(`${this.baseApiUrl}getUserById_${id}`)
  }
  adduser(useObj: any) {
    return this.http.post<any>(`${this.baseApiUrl}addUser`,useObj)
  }
  updateUser(userObj: any) {
    return this.http.put<any>(`${this.baseApiUrl}updateUser`, userObj);
  }
  removeUser(id: number) {
    return this.http.delete<any>(`${this.baseApiUrl}${id}`)
  }
}
