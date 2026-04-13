using Microsoft.EntityFrameworkCore;

using FleetManagementApi.Data;
using FleetManagementApi.DTOs;
using FleetManagementApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{


    private readonly FleetDbContext _context;

    public VehicleController(FleetDbContext context)
    {
        _context = context;

    }

    [HttpGet]
    public async Task<IActionResult> GetAll(

[FromQuery] string? status,
[FromQuery] string? Type,
[FromQuery] string? search)
    {


        var query = _context.Vehicle.Include(v => v.Technicians).AsQueryable();

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<VehicleStatus>(status, true, out var statusEnum))
            query = query.Where(v => v.Status == statusEnum);

        if (!string.IsNullOrEmpty(Type) && Enum.TryParse
        <VehicleType>(Type, true, out var TypeEnum))
            query = query.Where(v => v.Type == TypeEnum);


        if (!string.IsNullOrEmpty(search))
            query = query.Where(v =>
            v.FleetNumber.Contains(search) ||
             v.Registration.Contains(search) ||
             v.Manufacturer.Contains(search)

            );

        var Vehicle = await query
        .OrderBy(v => v.FleetNumber)
        .Select(v => MapToDto(v))
        .ToListAsync();

        return Ok(Vehicle);

    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = new
        {
            Total = await _context.Vehicle.CountAsync(),
            Critical = await _context.Vehicle
                .CountAsync(v => v.Status == VehicleStatus.Critical),
            Pending = await _context.Vehicle
                .CountAsync(v => v.Status == VehicleStatus.Pending),
            Cleared = await _context.Vehicle
                .CountAsync(v => v.Status == VehicleStatus.Cleared)
        };
        return Ok(stats);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vehicle = await _context.Vehicle
            .Include(v => v.Technicians)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vehicle is null) return NotFound();
        return Ok(MapToDto(vehicle));
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleDto dto)
    {
        if (!Enum.TryParse<VehicleType>(dto.Type, true, out var type))
            return BadRequest("Invalid vehicle type.");

        if (!Enum.TryParse<VehicleStatus>(dto.Status, true, out var status))
            return BadRequest("Invalid vehicle status");



        var vehicle = new Vehicle
        {

            FleetNumber = dto.FleetNumber,
            Registration = dto.Registration,
            Type = type,
            Manufacturer = dto.Manufacturer,
            Model = dto.Model,
            Status = status,
            LastServiceDate = dto.LastServiceDate,
            TechnicianId = dto.TechnicianId



        };

        _context.Vehicle.Add(vehicle);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
        new { id = vehicle.Id }, MapToDto(vehicle)


        );

    }

    // PUT api/vehicles/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateVehicleDto dto)
    {
        var vehicle = await _context.Vehicle.FindAsync(id);
        if (vehicle is null) return NotFound();

        if (!Enum.TryParse<VehicleType>(dto.Type, true, out var type))
            return BadRequest("Invalid vehicle type.");

        if (!Enum.TryParse<VehicleStatus>(dto.Status, true, out var status))
            return BadRequest("Invalid vehicle status.");

        vehicle.FleetNumber = dto.FleetNumber;
        vehicle.Registration = dto.Registration;
        vehicle.Type = type;
        vehicle.Manufacturer = dto.Manufacturer;
        vehicle.Model = dto.Model;
        vehicle.Status = status;
        vehicle.LastServiceDate = dto.LastServiceDate;
        vehicle.TechnicianId = dto.TechnicianId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/vehicles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vehicle = await _context.Vehicle.FindAsync(id);
        if (vehicle is null) return NotFound();

        _context.Vehicle.Remove(vehicle);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static VehicleDto MapToDto(Vehicle v) => new()
    {
        Id = v.Id,
        FleetNumber = v.FleetNumber,
        Registration = v.Registration,
        Type = v.Type.ToString(),
        Manufacturer = v.Manufacturer,
        Model = v.Model,
        Status = v.Status.ToString(),
        LastServiceDate = v.LastServiceDate,
        Technician = v.Technicians is null ? null : new TechnicianSummaryDto
        {
            Id = v.Technicians.Id,
            FullName = $"{v.Technicians.FirstName} {v.Technicians.LastName}",
            Role = v.Technicians.Role,
            Initials = v.Technicians.Initials,

        }
    };

    [HttpPost("bulk-delete")]
    public async Task<IActionResult> BulkDeleteVehicles([FromBody] List<int> ids)
    {
        if (ids == null || !ids.Any())
        {

            return BadRequest("No vehicle IDs were provided ");
        }

        var vehiclesToDelete = await _context.Vehicle

        .Where(v => ids.Contains(v.Id))
        .ToListAsync();


        if (!vehiclesToDelete.Any())
        {
            return NotFound("None of the specified vehicles could be found");
        }

        _context.Vehicle.RemoveRange(vehiclesToDelete);

        await _context.SaveChangesAsync();

        return Ok(new { message = $"{vehiclesToDelete.Count} vehicles successfully deleted." });
    }
    [HttpPut("{id}/technician")]
    public async Task<IActionResult> AssignTechnician(int id, [FromBody] AssignTechnicianDto request)
    {
        // 1. Find the specific truck in the database
        var vehicle = await _context.Vehicle.FindAsync(id);

        if (vehicle == null)
        {
            return NotFound();
        }

        // 2. Update its TechnicianId
        vehicle.TechnicianId = request.TechnicianId;

        // 3. Save the changes to MySQL!
        await _context.SaveChangesAsync();

        // 4. Send a thumbs up back to Angular (204 No Content means Success)
        return NoContent();
    }
}

public class AssignTechnicianDto
{
    public int? TechnicianId { get; set; }
}