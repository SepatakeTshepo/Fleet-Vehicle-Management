export interface Technician{

    Id :number
    FirstName : string
    LastName : string
    Role : string
    Initials :string



}

export interface Vehicle {

    Id : number
    FleetNumber :string 
    Registration : string 
    Type : string
    Manufacturer : string
    Model :string
    Status : String
    LastServiceDate ? :string | null
    Technician ?:Technician | null


}

export interface VehicleStats {

Total : number
Critical :number
Pending :number
Cleared :number

}

export interface CreateVehicle {
    FleetNumber: string
    Registration: string
    Type: string
    Manufacturer: string
    Model: string
    Status: string
    LastServiceDate ?: string | null
    TechnicianId ?: number | null
}
