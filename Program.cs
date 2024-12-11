// See https://aka.ms/new-console-template for more information
using EFCode9Demo;
using Microsoft.EntityFrameworkCore;

using (var context = new ApplicationDbContext())
{
    // 添加新项
    var newItem = new Item
    {
        Id = Guid.NewGuid().ToString(),
        PartitionKey = "CategoryA",
        Name = "Sample Item",
        Price = 9.99m
    };
    context.Items.Add(newItem);
    await context.SaveChangesAsync();

    // 查询项
    var items = await context.Items
        .Where(i => i.PartitionKey == "CategoryA" && i.Price > 5.00m)
        .ToListAsync();

    // 删除项
    context.Items.Remove(newItem);
    await context.SaveChangesAsync();
}
