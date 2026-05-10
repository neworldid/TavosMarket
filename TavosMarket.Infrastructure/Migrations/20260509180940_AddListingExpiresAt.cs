using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavosMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddListingExpiresAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "Listings",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "Listings");
        }
    }
}
