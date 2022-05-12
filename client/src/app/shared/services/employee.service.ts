import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  public baseApiUrl:string = "https://localhost:7232/Employee/"
  constructor(private http: HttpClient) { }

  getAllEmployee() {
    return this.http.get<any>(this.baseApiUrl)
  }
  employeeCode(code: string) {
    return this.http.get<any>(`${this.baseApiUrl}getEmployeeByCode_${code}`)
  }
  employeeById(id: number) {
    return this.http.get<any>(`${this.baseApiUrl}getEmployeeById_${id}`)
  }
  employeeName(name: string) {
    return this.http.get<any>(`${this.baseApiUrl}getEmployeeByName_${name}`)
  }
  addEmployee(useObj: any) {
    return this.http.post<any>(`${this.baseApiUrl}addEmployee`,useObj)
  }
  updateEmployee(userObj: any) {
    return this.http.put<any>(`${this.baseApiUrl}updateEmployee`, userObj);
  }
  removeUser(id: number) {
    return this.http.delete<any>(`${this.baseApiUrl}${id}`)
  }
}
