namespace FleetManagementApi.DTOs;


public class VehicleDto
{

    public int Id { get; set; }
    public string FleetNumber { get; set; } = string.Empty;
    public string Registration { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime LastServiceDate { get; set; }
    public TechnicianSummaryDto? Technician { get; set; }


}


public class CreateVehicleDto
{

    public int Id { get; set; }
    public string FleetNumber { get; set; } = string.Empty;
    public string Registration { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime LastServiceDate { get; set; }
    public int? TechnicianId { get; set; }
}


public class TechnicianSummaryDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
}