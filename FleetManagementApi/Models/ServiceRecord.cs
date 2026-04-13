namespace FleetManagementApi.Models;

public class ServiceRecord
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public DateTime ServiceDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ServicedBy { get; set; } = string.Empty;
    public decimal Cost { get; set; }


    public Vehicle vehicle { get; set; } = null!;

}