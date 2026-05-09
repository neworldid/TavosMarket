using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavosMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDirectUseForListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDirectUseForListings",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDirectUseForListings",
                table: "Categories");
        }
    }
}
