import { Component, inject, OnInit ,ChangeDetectorRef } from "@angular/core";
import {Technician, Vehicle} from "../../Models/vehicle.models";
import { RouterLink } from '@angular/router';
import { VehicleService } from "../../Service/Vehicle.service";
import { VehicleStats } from "../../Models/vehicle.models";
import { CreateVehicle } from "../../Models/vehicle.models";
import { FormsModule } from '@angular/forms';
import { DatePipe } from "@angular/common";
import { forkJoin } from 'rxjs';
import { DashboardComponent } from "../dashboard/dashboard";

@Component({
  selector: 'app-vehicles',
  imports: [FormsModule, DatePipe , DashboardComponent],

  standalone :true,
  templateUrl: './vehicles.html',
  styleUrls:  ['./vehicles.css']
})

export class Vehicles implements OnInit{


  
  

  private VehicleService = inject(VehicleService);
private cdr = inject(ChangeDetectorRef);
  
  selectedVehicleIds: number[] = [];

  
  toggleSelection(id: number, event: any) {
    if (event.target.checked) {
      
      this.selectedVehicleIds.push(id);
    } else {
     
      this.selectedVehicleIds = this.selectedVehicleIds.filter(selectedId => selectedId !== id);
    }
  }

  vehicleList : Vehicle[] = [];
  technicianList : Technician[]=[];
  stats : VehicleStats = {Total : 0 , Critical :0 , Pending : 0 , Cleared : 0};

  isImportModalOpen = false;

  openImportModal(){
    this.isImportModalOpen=true;
  }

  closeImportModal(){

    this.isImportModalOpen =false;
  }


ngOnInit() {
  this.loadBackendData();
}


loadBackendData (){

  this.VehicleService.getVehicle().subscribe({

    next: (data) => {
      console.log ("Here is the data  from C# " , data);
      this.vehicleList = data;
this.cdr.detectChanges();

    }});

    this.VehicleService.getTechnician().subscribe({
      next: (data) => {
        console.log("Technicians from C#:", data); 
        this.technicianList = data;
        this.cdr.detectChanges(); 
      },
    error: (err) => console.error ("Failed to load vehicles from backend , err")
  });

  this.VehicleService.getTechnician().subscribe({
next: (data) => this.technicianList = data ,
error : (err) => console.error ("Failed to lead techs , err")

  })
  

}
  isAddModalOpen = false;

  openAddModal() {
    this.isAddModalOpen = true;
  }

  closeAddModal() {
    this.isAddModalOpen = false;
  }


  
  clearSelection() {
    this.selectedVehicleIds = []; 
  }

  // 2. The DELETE button
 deleteSelectedVehicles() {
    if (confirm(`Are you sure you want to delete ${this.selectedVehicleIds.length} vehicles?`)) {
      
      // Create an array of HTTP Delete requests
      const deleteRequests = this.selectedVehicleIds.map(id => 
        this.VehicleService.deleteVehicle(id)
      );

      // forkJoin runs them all at once!
      forkJoin(deleteRequests).subscribe({
        next: () => {
          this.VehicleService.refreshStatsSignal.next(true);
          this.clearSelection(); // Empty the bucket (hides the green bar)
          this.loadBackendData(); // Refresh the table
        },
        error: (err) => console.error('Failed to delete vehicles', err)
      });
    }
  }
  // 3. The ASSIGN TECHNICIAN Modal State
  isAssignTechModalOpen = false;

  openAssignTechModal() {
    this.isAssignTechModalOpen = true;
    console.log("Opening technician modal for vehicles:", this.selectedVehicleIds);
  }

  closeAssignTechModal() {
    this.isAssignTechModalOpen = false;
  }

 
  bulkTechnicianId: number | null = null; 

  
  confirmAssignTechnician() {
  
    if (this.selectedVehicleIds.length === 0) return;

  
    const assignRequests = this.selectedVehicleIds.map(id => 
      this.VehicleService.assignTechnician(id, this.bulkTechnicianId)
    );

   
    forkJoin(assignRequests).subscribe({
      next: () => {
        this.closeAssignTechModal(); 
        this.clearSelection(); 
        this.loadBackendData(); 
      },
      error: (err) => console.error('Failed to assign technicians', err)
    });

    
  }

// --- VIEW MODAL LOGIC ---
  isViewModalOpen = false;
  selectedVehicleToView: Vehicle | null = null; 

  openViewModal(vehicle: Vehicle) {
    this.selectedVehicleToView = vehicle;  
    this.isViewModalOpen = true;         
  }

  closeViewModal() {
    this.isViewModalOpen = false;
    this.selectedVehicleToView = null;   
  }

/* EDIT MODAL LOGIC */
  isEditModalOpen = false;
  
 
  editVehicleForm: any = {}; 

  openEditModal(vehicle: Vehicle) {
    
    this.editVehicleForm = { ...vehicle }; 
    
    this.isEditModalOpen = true;
    this.closeViewModal();
  }

  closeEditModal() {
    this.isEditModalOpen = false;
  }

  saveEditedVehicle() {
    
    this.VehicleService.updateVehicle(this.editVehicleForm.Id, this.editVehicleForm).subscribe({
      next: () => {
        
        const index = this.vehicleList.findIndex(v => v.Id === this.editVehicleForm.Id);
        if (index !== -1) {
          this.vehicleList[index] = { ...this.editVehicleForm };
        }

       
        this.closeEditModal();

        
        this.cdr.detectChanges();
      },
      error: (err) => console.error("Failed to update vehicle", err)
    });
  }



newVehicleForm : CreateVehicle = {

  FleetNumber: "",
  Registration:"",
  Type :" Trailer",
  Manufacturer:"",
  Model :"",
  Status:"Pending ",
  LastServiceDate:null,
  TechnicianId: null

};

registerVehicle() {
    this.VehicleService.createVehicle(this.newVehicleForm).subscribe({
      next: () => {
        
        this.closeAddModal();
        this.loadBackendData();
      
        this.newVehicleForm = {
          FleetNumber: '', Registration: '', Type: 'Truck', 
          Manufacturer: '', Model: '', Status: 'Pending', LastServiceDate: null, TechnicianId: null
        };
      },
      error: (err) => console.error('Failed to add vehicle', err)
    });
  }
}
