using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavosMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRedundantColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CategoryFieldDefinitions");

            migrationBuilder.DropColumn(
                name: "IsSearchable",
                table: "CategoryFieldDefinitions");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CategoryFieldDefinitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSearchable",
                table: "CategoryFieldDefinitions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
