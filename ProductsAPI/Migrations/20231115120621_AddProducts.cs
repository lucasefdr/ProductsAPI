using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsAPI.Migrations
{
    public partial class AddProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO products(name, description, price, imageurl, stock, registrationdate, categoryid)  " +
                "VALUES('Coke', 'Coke soda 350ml', 2.39, 'coke350.jpg', 50, now(), 2)");
            migrationBuilder.Sql("INSERT INTO products(name, description, price, imageurl, stock, registrationdate, categoryid)  " +
                "VALUES('Tuna Snack', 'Tuna snack with mayonnaise', 4.40, 'tunasnack.jpg', 10, now(), 1)");
            migrationBuilder.Sql("INSERT INTO products(name, description, price, imageurl, stock, registrationdate, categoryid)  " +
                "VALUES('Pudding', 'Pudding 200g', 3.59, 'pudding200.jpg', 22, now(), 3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM products");
        }
    }
}
