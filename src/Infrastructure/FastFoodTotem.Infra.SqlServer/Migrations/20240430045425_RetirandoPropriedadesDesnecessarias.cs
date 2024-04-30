using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodTotem.Infra.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class RetirandoPropriedadesDesnecessarias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "FastFoodTotem",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "InStoreOrderId",
                schema: "FastFoodTotem",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InStoreOrderId",
                schema: "FastFoodTotem",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "FastFoodTotem",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
