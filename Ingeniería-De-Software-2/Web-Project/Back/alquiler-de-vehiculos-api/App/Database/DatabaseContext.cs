
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options) {}

        public DbSet<Additional> Additionals { get; set; }
        public DbSet<AdditionalDriver> AdditionalDrivers { get; set; }
        public DbSet<AdditionalRental> AdditionalRentals { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<CancellationPolicy> CancellationPolicies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }
        public DbSet<VehicleStatus> VehicleStatuses { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasKey(d => d.UserId);
            modelBuilder.Entity<Employee>()
                .HasKey(d => d.UserId);
            modelBuilder.Entity<Customer>()
                .HasKey(d => d.UserId);
        }

    }
}