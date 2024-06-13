using Assesment.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Assesment.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }

        public void SeedData()
        {
            // Clear existing data
            if (Items.Any())
            {
                Items.RemoveRange(Items);
            }
            if (Receipts.Any())
            {
                Receipts.RemoveRange(Receipts);
            }
            if (ReceiptItems.Any())
            {
                ReceiptItems.RemoveRange(ReceiptItems);
            }

            SaveChanges();

            // Add new items
            Items.AddRange(
                new Item { Name = "Item1", Price = 10.00m, Quantity = 100 },
                new Item { Name = "Item2", Price = 20.00m, Quantity = 50 },
                new Item { Name = "Item3", Price = 30.00m, Quantity = 30 }
            );

            SaveChanges();
        }
    }

}
