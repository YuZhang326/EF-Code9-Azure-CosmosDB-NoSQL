using Microsoft.EntityFrameworkCore;

namespace EFCode9Demo;

public class ApplicationDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(
            "https://<your-account-uri>",
            "<your-account-key>",
            databaseName: "Yu-Cloud-Database");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>().ToContainer("Items");
        modelBuilder.Entity<Item>().HasPartitionKey(i => i.PartitionKey);
    }
}
