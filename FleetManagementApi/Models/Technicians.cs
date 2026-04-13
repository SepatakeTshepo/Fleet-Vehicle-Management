namespace FleetManagementApi.Models;

public class Technicians
{

    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public string Initials { get; set; } = string.Empty;


    public ICollection<Vehicle> Vehicle { get; set; } = new List<Vehicle>();





}