# Azure Cosmos DB with EF Core 9 Example

This README provides step-by-step instructions to create a .NET application that integrates with Azure Cosmos DB for NoSQL using Entity Framework Core 9.

## Prerequisites

- **Azure Subscription**: You need an active Azure subscription to create an Azure Cosmos DB account.
- **.NET SDK**: Install the latest version of the .NET SDK.
- **Development Environment**: Visual Studio, Visual Studio Code, or any preferred IDE.

## Steps

### 1. Create Azure Cosmos DB Account

1. Log in to the [Azure Portal](https://portal.azure.com/).
2. Create a new Azure Cosmos DB account.
   - Select the **NoSQL** API.
   - Note down the **Account URI** and **Primary Key** for your database.

### 2. Create a .NET Project

1. Open your terminal or IDE.
2. Run the following command to create a new .NET project:

   ```bash
   dotnet new console -n CosmosDBExample
   cd CosmosDBExample
   ```

3. Install the EF Core package for Azure Cosmos DB:

   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.Cosmos
   ```

### 3. Configure `DbContext`

Create a `DbContext` class to configure the database connection and entity mapping.

```csharp
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(
            "https://<your-account-uri>",
            "<your-account-key>",
            databaseName: "YourDatabaseName");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>().ToContainer("Items");
        modelBuilder.Entity<Item>().HasPartitionKey(i => i.PartitionKey);
    }
}
```

Replace `<your-account-uri>` and `<your-account-key>` with the values from your Azure Cosmos DB account.

### 4. Define Entity

Create a model class to represent your data.

```csharp
public class Item
{
    public string Id { get; set; }
    public string PartitionKey { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

### 5. Perform Database Operations

Write a program to perform CRUD operations.

```csharp

using (var context = new ApplicationDbContext())
{
    // Add new item
    var newItem = new Item
    {
        Id = Guid.NewGuid().ToString(),
        PartitionKey = "CategoryA",
        Name = "Sample Item",
        Price = 9.99m
    };
    context.Items.Add(newItem);
    await context.SaveChangesAsync();

    // Query items
    var items = await context.Items
        .Where(i => i.PartitionKey == "CategoryA" && i.Price > 5.00m)
        .ToListAsync();
    Console.WriteLine("Items found: " + items.Count);

    // Delete item
    context.Items.Remove(newItem);
    await context.SaveChangesAsync();
}

```

### 6. Run the Application

1. Build and run the application:

   ```bash
   dotnet run
   ```

2. Verify the operations in the Azure Portal.

## Notes

- Ensure that your Azure Cosmos DB account is properly configured to allow connections from your application.
- Use the partition key wisely to optimize database performance.
- Always use async methods when interacting with Azure Cosmos DB to avoid blocking threads.

## References

- [Azure Cosmos DB Documentation](https://learn.microsoft.com/en-us/azure/cosmos-db/)
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
