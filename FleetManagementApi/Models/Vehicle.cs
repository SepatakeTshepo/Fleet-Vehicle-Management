using FleetManagementApi.DTOs;

namespace FleetManagementApi.Models;

public enum VehicleStatus { Critical, Pending, Cleared }
public enum VehicleType { Truck, Trailer, Van }




public class Vehicle
{
    public int Id { get; set; }
    public string FleetNumber { get; set; } = string.Empty;
    public string Registration { get; set; } = string.Empty;
    public VehicleType Type { get; set; }

    public string Manufacturer { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;
    public VehicleStatus Status { get; set; }
    public DateTime LastServiceDate { get; set; }

    public int? TechnicianId { get; set; }

    public Technicians? Technicians { get; set; }




}