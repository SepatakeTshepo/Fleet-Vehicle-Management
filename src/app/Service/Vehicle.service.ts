import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CreateVehicle, Vehicle, VehicleStats , Technician} from "../Models/vehicle.models";
import { BehaviorSubject } from 'rxjs';
@Injectable({
providedIn :"root"// makes it available anywhere
})

export class VehicleService {

  public refreshStatsSignal = new BehaviorSubject<boolean>(true);
private http = inject (HttpClient)

private apiUrl = 'http://localhost:5019/api/Vehicle'
private technicianUrl = 'http://localhost:5019/api/Technicians';
getVehicle(Status? :string , Type? :string , Search?  :string):
Observable <Vehicle[]>{

let params = new HttpParams()

if (Status)params = params.set("status", Status)
    if(Type) params = params.set ("Type" , Type)
        if (Search) params = params.set ("Search" ,Search)

            return this.http.get<Vehicle []>(this.apiUrl , {params})

}

getTechnician(): Observable<Technician[]>{

   return this.http.get<Technician[]>(this.technicianUrl); 
  }

getStats ():Observable <VehicleStats>{

return this.http.get<VehicleStats>(`${this.apiUrl}/stats`)

}

createVehicle (vehicle:CreateVehicle) :Observable <Vehicle>{

return this.http.post<Vehicle>(this.apiUrl , vehicle)
}

updateVehicle (id :number , vehicle:CreateVehicle):Observable <void>{

return this.http.put<void>(`${this.apiUrl}/${id}` , vehicle)

}

deleteVehicle(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`,{ responseType: 'text' });
  }

  assignTechnician(vehicleId: number, technicianId: number | null): Observable<any> {
    return this.http.put(`${this.apiUrl}/${vehicleId}/technician`, { technicianId: technicianId },{ responseType: 'text' });
  }


}