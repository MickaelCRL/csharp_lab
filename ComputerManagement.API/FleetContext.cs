using Microsoft.EntityFrameworkCore;
using ComputerManagement.Shared;
using ComputerManagement.Shared.Computers;

namespace ComputerManagement.API;

public class FleetContext : DbContext 
{
	public virtual DbSet<Computer> Computers { get; set; }
    public DbSet<Repair> Repairs { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
	{
		optionsBuilder.UseSqlite("Data Source=fleet.db");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Computer>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Computer>()
			.HasDiscriminator<string>("ComputerType")
			.HasValue<Laptop>("laptop")
			.HasValue<Desktop>("desktop");

        modelBuilder.Entity<Repair>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Repair>()
        .HasOne(r => r.Computer)
        .WithMany()
        .HasForeignKey(r => r.ComputerId);
    }
}
