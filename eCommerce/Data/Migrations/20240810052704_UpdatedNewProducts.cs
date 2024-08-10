using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerce.Data.Migrations
{
    public partial class UpdatedNewProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat001",
                column: "Name",
                value: "Espresso Beans");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat002",
                column: "Name",
                value: "Dark Roast");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat003",
                column: "Name",
                value: "Light Roast");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "Cat004", "Medium Roast" },
                    { "Cat005", "Decaf" }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Image", "Name", "Price" },
                values: new object[] { "Rich and bold premium espresso beans.", "espresso_roast.jpg", "Premium Espresso Beans", 14.99m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Cat002", "Intense and smoky dark roast coffee.", "dark_roast.jpg", "French Dark Roast", 13.99m, 120 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Cat003", "Bright and fruity light roast blend.", "light_roast.jpg", "Light Roast Blend", 12.99m, 150 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Cat004", "Well-balanced medium roast coffee.", "medium_roast.jpg", "Classic Medium Roast", 13.49m, 130 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Cat005", "Smooth and flavorful decaf coffee.", "decaf_coffee.jpg", "Decaf Coffee", 11.99m, 140 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat004");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat005");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat001",
                column: "Name",
                value: "Espresso");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat002",
                column: "Name",
                value: "Cold Brew");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat003",
                column: "Name",
                value: "French Press");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Image", "Name", "Price" },
                values: new object[] { "Rich and bold espresso beans, perfect for a strong shot of espresso.", "espresso_beans.jpg", "Espresso Beans", 12.99m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Cat001", "A double shot of strong and rich espresso.", "double_espresso.jpg", "Double Espresso", 2.99m, 150 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Cat002", "Smooth and refreshing cold brew coffee.", "cold_brew.jpg", "Cold Brew Coffee", 4.99m, 75 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Cat002", "Cold brew coffee infused with nitrogen for a creamy texture.", "nitro_cold_brew.jpg", "Nitro Cold Brew", 5.99m, 60 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Cat003", "Coarsely ground coffee beans, ideal for French press brewing.", "french_press_coffee.jpg", "French Press Coffee", 14.99m, 80 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { 6, "Cat003", "Organic, fair-trade coffee beans for French press brewing.", "organic_french_press_coffee.jpg", "Organic French Press Coffee", 16.99m, 70 });
        }
    }
}
