using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerce.Data.Migrations
{
    public partial class UpdatedCoffeeProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Rich and bold espresso beans, perfect for a strong shot of espresso.", "espresso_beans.jpg", "Espresso Beans", 12.99m, 100 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "A double shot of strong and rich espresso.", "double_espresso.jpg", "Double Espresso", 2.99m, 150 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Smooth and refreshing cold brew coffee.", "cold_brew.jpg", "Cold Brew Coffee", 4.99m, 75 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Cat002", "Cold brew coffee infused with nitrogen for a creamy texture.", "nitro_cold_brew.jpg", "Nitro Cold Brew", 5.99m, 60 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 5, "Cat003", "Coarsely ground coffee beans, ideal for French press brewing.", "french_press_coffee.jpg", "French Press Coffee", 14.99m, 80 },
                    { 6, "Cat003", "Organic, fair-trade coffee beans for French press brewing.", "organic_french_press_coffee.jpg", "Organic French Press Coffee", 16.99m, 70 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat001",
                column: "Name",
                value: "Electronics");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat002",
                column: "Name",
                value: "Books");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Cat003",
                column: "Name",
                value: "Clothing");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "A high-performance laptop.", "laptop.jpg", "Laptop", 999.99m, 50 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "A latest model smartphone.", "smartphone.jpg", "Smartphone", 499.99m, 100 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "A thrilling mystery novel.", "book.jpg", "Mystery Novel", 15.99m, 200 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "Image", "Name", "Price", "Stock" },
                values: new object[] { "Cat003", "A warm winter jacket.", "jacket.jpg", "Winter Jacket", 89.99m, 30 });
        }
    }
}
