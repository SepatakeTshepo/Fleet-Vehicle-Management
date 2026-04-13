namespace FleetManagementApi.DTOs;

//data we send to user
public class TechnicianDto
{

    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;


    public string Initials { get; set; } = string.Empty;

    public int VehicleCount { get; set; }
}

//what we receive from the client
public class CreateTechnicianDto
{

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Initials { get; set; } = string.Empty;

}
