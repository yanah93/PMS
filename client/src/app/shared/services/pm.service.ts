import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PmService {

  public baseApiUrl:string = "https://localhost:7232/api/PMManager/"

  constructor(private http : HttpClient) { }

  addPM(formData : any){
    return this.http.post<any>(`${this.baseApiUrl}add`,formData);
  }
  getAllPMs(){
    return this.http.get<any>(this.baseApiUrl);
  }
  updatePM(empObj : any){
    return this.http.put<any>(`${this.baseApiUrl}update`,empObj);
  }
  deletePM(id:number){
    return this.http.delete<any>(`${this.baseApiUrl}${id}`)
  }
  getPMbyId(id:number){
    return this.http.get<any>(`${this.baseApiUrl}${id}`);
  }
}
