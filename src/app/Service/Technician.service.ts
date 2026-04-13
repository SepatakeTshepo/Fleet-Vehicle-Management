import { HttpClient } from "@angular/common/http"
import {  Injectable, inject} from "@angular/core"
import { Observable } from "rxjs"
import { Technician } from "../Models/vehicle.models"

export interface TechnicianDto {

Id : number
FirstName :string
SecondName :string
Role :string
Initials:string
VehicleCount :Number

}


export interface CreateTechnician {

FirstName :string
LastName :string
Role :string
Initiials :string


}

@Injectable({

    providedIn:"root"
})

export class TechnicianService{

    private http =inject (HttpClient)
    private apiUrl='http://localhost:5019/api/Technicians'

 getTechnicans() : Observable<TechnicianDto[]> {

return this.http.get<TechnicianDto[]>(this.apiUrl)}


getTechnicianById (Id :number) : Observable <TechnicianDto>{

return this.http.get<TechnicianDto>("${this.apiUrl}/${id}")

}



createTechnician(technician: CreateTechnician): Observable<TechnicianDto> {
    return this.http.post<TechnicianDto>(this.apiUrl, technician)
  }

  updateTechnician(id: number, technician: CreateTechnician): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, technician)
  }

  deleteTechnician(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
  }
 


}