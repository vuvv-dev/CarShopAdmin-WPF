using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppContext;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<CarBranch> CarBranches { get; set; }
    public DbSet<CarSample> CarSamples { get; set; }
    public DbSet<CarStatus> CarStatuses { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<BillStatus> BillStatuses { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarStatus>(entity =>
        {
            entity.HasData(
                new CarStatus { Id = 1, Status = "In Stock" },
                new CarStatus { Id = 2, Status = "Out of Stock" },
                new CarStatus { Id = 3, Status = "Importing" }
            );
        });

        modelBuilder.Entity<CarSample>(entity =>
        {
            entity.HasData(
                new CarSample { Id = 1, SampleName = "Oto" },
                new CarSample { Id = 2, SampleName = "Sedan" },
                new CarSample { Id = 3, SampleName = "SUV" },
                new CarSample { Id = 4, SampleName = "Truck" },
                new CarSample { Id = 5, SampleName = "Convertible" },
                new CarSample { Id = 6, SampleName = "Hatchback" },
                new CarSample { Id = 7, SampleName = "Coupe" },
                new CarSample { Id = 8, SampleName = "MPV" }
            );
        });

        modelBuilder.Entity<BillStatus>(entity =>
        {
            entity.HasData(
                new BillStatus { Id = 1, BillStatusName = "Unpaid" },
                new BillStatus { Id = 2, BillStatusName = "Paid" }
            );
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasData(
                new User
                {
                    Id = 1,
                    FullName = "Admin",
                    Username = "admin",
                    Password = "admin",
                    Role = "admin",
                }
            );
        });

        modelBuilder
            .Entity<Car>()
            .HasOne(car => car.CarBranch)
            .WithMany(branch => branch.Cars)
            .HasForeignKey(car => car.CarBranchId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuring foreign key for Car -> CarStatus
        modelBuilder
            .Entity<Car>()
            .HasOne(car => car.CarStatus)
            .WithMany(status => status.Cars)
            .HasForeignKey(car => car.CarStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuring foreign key for Car -> CarSample
        modelBuilder
            .Entity<Car>()
            .HasOne(car => car.CarSample)
            .WithMany(sample => sample.Cars)
            .HasForeignKey(car => car.CarSampleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
