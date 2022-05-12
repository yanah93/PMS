import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  public baseApiUrl:string = "https://localhost:7232/Users/"
  constructor(private http: HttpClient) { }

  getAllUsers() {
    return this.http.get<any>(this.baseApiUrl)
  }
  profile(id: number) {
    return this.http.get<any>(`${this.baseApiUrl}getUserById${id}`)
  }
  adduser(formData: any) {
    return this.http.post<any>(`${this.baseApiUrl}add`,formData)
  }
  updateUser(userObj: any) {
    return this.http.put<any>(`${this.baseApiUrl}update`, userObj);
  }
  removeUser(id: number) {
    return this.http.delete<any>(`${this.baseApiUrl}${id}`)
  }
}
