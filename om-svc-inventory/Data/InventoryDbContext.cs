using Microsoft.EntityFrameworkCore;
using om_svc_inventory.Models;
using System.Reflection.Metadata;

namespace om_svc_inventory.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions options) : base(options) { }

        public DbSet<InventoryCategory> InventoryCategories { get; set; }

        public DbSet<InventoryItem> InventoryItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryItem>().HasData(
               new InventoryItem()
               {
                   Id = new Guid("c7e739f3-9f8d-4d20-992d-8e2c6638e947"),
                   CategoryId = new Guid("19386c96-8197-48fd-b89c-01d52eb5ad73"),
                   Name = "Burgundy Folding",
                   Price = 0.60m,
               },
               new InventoryItem()
               {
                   Id = new Guid("7081f119-c671-4898-920e-7536248c0753"),
                   CategoryId = new Guid("9f6e6232-43e3-4c14-b99d-e5485af44cd9"),
                   Name = "8' Banguet",
                   Price = 12.50m,
               },
               new InventoryItem()
               {
                   Id = new Guid("11ff3db8-bce2-4cf0-bfb9-152f3ac70f75"),
                   CategoryId = new Guid("7b58054d-2fbb-4b82-a14f-4a7d4ad076db"),
                   Name = "20 X 30",
                   Price = 250.00m,
               },
               new InventoryItem()
               {
                   Id = new Guid("4c6fda94-34de-4fc8-889d-49c1b8d778c6"),
                   CategoryId = new Guid("c53eac40-eff4-49bd-a18d-761f50975aae"),
                   Name = "20 X 40",
                   Price = 375.00m,
               }
            );

            modelBuilder.Entity<InventoryCategory>()
           .Property(b => b.Description)
           .IsRequired(false);
 
            modelBuilder.Entity<InventoryCategory>().HasData(
               new InventoryCategory()
               {
                   Id = new Guid("19386c96-8197-48fd-b89c-01d52eb5ad73"),
                   Name = "Chair"
               },
               new InventoryCategory()
               {
                   Id = new Guid("9f6e6232-43e3-4c14-b99d-e5485af44cd9"),
                   Name = "Table"
               },
               new InventoryCategory()
               {
                   Id = new Guid("7b58054d-2fbb-4b82-a14f-4a7d4ad076db"),
                   Name = "Pole Tent"
               },
               new InventoryCategory()
               {
                   Id = new Guid("c53eac40-eff4-49bd-a18d-761f50975aae"),
                   Name = "Frame Tent"
               }
           );

        }
    }
}
