using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShoppingCart.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusIdinOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "OrderStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "OrderStatuses");
        }
    }
}
