using Microsoft.EntityFrameworkCore;
using ComputerManagement.Shared;

namespace ComputerManagement.API;

public class FleetContext : DbContext 
{
	public virtual DbSet<Computer> Computers { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
	{
		optionsBuilder.UseSqlite("Data Source=fleet.db");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<Computer>()
			.HasDiscriminator<string>("ComputerType")
			.HasValue<Laptop>("laptop")
			.HasValue<Desktop>("desktop");
	}
}
