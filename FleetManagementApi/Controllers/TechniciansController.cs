using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetManagementApi.Data;
using FleetManagementApi.DTOs;
using FleetManagementApi.Models;

namespace FleetManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TechniciansController : ControllerBase
{
    private readonly FleetDbContext _context;

    public TechniciansController(FleetDbContext context)
    {
        _context = context;
    }

    // GET api/technicians
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var technicians = await _context.Technicians
            .Include(t => t.Vehicle)
            .Select(t => new TechnicianDto
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,

                Role = t.Role,
                Initials = t.Initials,

                VehicleCount = t.Vehicle.Count
            })
            .ToListAsync();

        return Ok(technicians);
    }

    // GET api/technicians/1
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int Id)
    {
        var technician = await _context.Technicians
            .Include(t => t.Vehicle)
            .FirstOrDefaultAsync(t => t.Id == Id);

        if (technician is null) return NotFound();

        return Ok(new TechnicianDto
        {
            Id = technician.Id,
            FirstName = technician.FirstName,
            LastName = technician.LastName,

            Role = technician.Role,
            Initials = technician.Initials,

            VehicleCount = technician.Vehicle.Count
        });
    }

    // POST api/technicians
    [HttpPost]
    public async Task<IActionResult> Create(CreateTechnicianDto dto)
    {
        var technician = new Technicians
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role,
            Initials = dto.Initials,

        };

        _context.Technicians.Add(technician);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { Id = technician.Id }, technician);
    }

    // PUT api/technicians/1
    [HttpPut("{Id}")]
    public async Task<IActionResult> Update(int Id, CreateTechnicianDto dto)
    {
        var technician = await _context.Technicians.FindAsync(Id);
        if (technician is null) return NotFound();

        technician.FirstName = dto.FirstName;
        technician.LastName = dto.LastName;
        technician.Role = dto.Role;
        technician.Initials = dto.Initials;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/technicians/1
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        var technician = await _context.Technicians.FindAsync(Id);
        if (technician is null) return NotFound();

        _context.Technicians.Remove(technician);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}