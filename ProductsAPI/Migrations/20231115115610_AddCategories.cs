using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsAPI.Migrations
{
    public partial class AddCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO categories(name, imageurl) VALUES('Snacks', 'snacks.jpg')");
            migrationBuilder.Sql("INSERT INTO categories(name, imageurl) VALUES('Beverages', 'beverages.jpg')");
            migrationBuilder.Sql("INSERT INTO categories(name, imageurl) VALUES('Desserts', 'desserts.jpg')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM categories"); 
        }
    }
}
