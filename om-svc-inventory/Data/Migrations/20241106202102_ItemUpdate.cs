using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace om_svc_inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class ItemUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "InventoryCategories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("19386c96-8197-48fd-b89c-01d52eb5ad73"), null, "Chair" },
                    { new Guid("7b58054d-2fbb-4b82-a14f-4a7d4ad076db"), null, "Pole Tent" },
                    { new Guid("9f6e6232-43e3-4c14-b99d-e5485af44cd9"), null, "Table" },
                    { new Guid("c53eac40-eff4-49bd-a18d-761f50975aae"), null, "Frame Tent" }
                });

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "Id", "CategoryId", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("11ff3db8-bce2-4cf0-bfb9-152f3ac70f75"), new Guid("7b58054d-2fbb-4b82-a14f-4a7d4ad076db"), "20 X 30", 250.00m },
                    { new Guid("4c6fda94-34de-4fc8-889d-49c1b8d778c6"), new Guid("c53eac40-eff4-49bd-a18d-761f50975aae"), "20 X 40", 375.00m },
                    { new Guid("7081f119-c671-4898-920e-7536248c0753"), new Guid("9f6e6232-43e3-4c14-b99d-e5485af44cd9"), "8' Banguet", 12.50m },
                    { new Guid("c7e739f3-9f8d-4d20-992d-8e2c6638e947"), new Guid("19386c96-8197-48fd-b89c-01d52eb5ad73"), "Burgundy Folding", 0.60m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryCategories");

            migrationBuilder.DropTable(
                name: "InventoryItems");
        }
    }
}
