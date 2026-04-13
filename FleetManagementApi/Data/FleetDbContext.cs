using Microsoft.EntityFrameworkCore;
using FleetManagementApi.Models;


namespace FleetManagementApi.Data;

public class FleetDbContext : DbContext
{

    public FleetDbContext(DbContextOptions<FleetDbContext> options)
    : base(options) { }

    public DbSet<Vehicle> Vehicle { get; set; }
    public DbSet<Technicians> Technicians { get; set; }
    public DbSet<ServiceRecord> ServiceRecords { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>(entity =>
        {


            entity.HasKey(v => v.Id);
            entity.Property(v => v.FleetNumber).IsRequired().HasMaxLength(20);
            entity.Property(v => v.Registration).IsRequired().HasMaxLength(20);

            entity.Property(v => v.Model).IsRequired().HasMaxLength(30);
            entity.Property(v => v.Manufacturer).IsRequired().HasMaxLength(50);

            entity.Property(v => v.Status).HasConversion<string>();
            entity.Property(v => v.Type).HasConversion<string>();

            entity.HasOne(v => v.Technicians)
                             .WithMany(t => t.Vehicle)
                             .HasForeignKey(v => v.TechnicianId)
                             .OnDelete(DeleteBehavior.SetNull);
        });


        modelBuilder.Entity<Technicians>(entity =>
        {

            entity.HasKey(t => t.Id);
            entity.Property(t => t.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(t => t.LastName).IsRequired().HasMaxLength(50);
            entity.Property(t => t.Role).IsRequired().HasMaxLength(50);
            entity.Property(t => t.Initials).IsRequired().HasMaxLength(50);
        }

        );

        // initialiizng the data from the get go called "Seed"
        modelBuilder.Entity<Technicians>().HasData(

        new Technicians
        {
            Id = 1,
            FirstName = "John",
            LastName = "Smith",
            Role = "Junior Tech",
            Initials = "JS"
        },

         new Technicians
         {
             Id = 2,
             FirstName = "Sarah",
             LastName = "Venter",
             Role = "Tech",
             Initials = "SV"
         }



        );

        modelBuilder.Entity<Vehicle>().HasData(
        new Vehicle
        {
            Id = 1,
            FleetNumber = "TRK-067",
            Registration = "L 114 829",
            Type = VehicleType.Truck,
            Manufacturer = "Iveco",
            Model = "Stralis",
            Status = VehicleStatus.Critical,
            LastServiceDate = new DateTime(2025, 6, 15),
            TechnicianId = 1
        },

        new Vehicle
        {
            Id = 2,
            FleetNumber = "TRL-021",
            Registration = "CA 987 654",
            Type = VehicleType.Trailer,
            Manufacturer = "Schmitz",
            Model = "S.KO",
            Status = VehicleStatus.Pending,
            LastServiceDate = new DateTime(2024, 11, 18),
            TechnicianId = 2
        }

        );




    }
}

