using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavosMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingFavorites");

            migrationBuilder.DropIndex(
                name: "IX_CategoryFieldOptions_FieldDefinitionId",
                table: "CategoryFieldOptions");

            migrationBuilder.DropIndex(
                name: "IX_CategoryFieldDefinitions_CategoryId",
                table: "CategoryFieldDefinitions");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatedAtUtc",
                table: "Listings",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Price",
                table: "Listings",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Status",
                table: "Listings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Status_CategoryId_CreatedAtUtc",
                table: "Listings",
                columns: new[] { "Status", "CategoryId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFieldOptions_FieldDefinitionId_SortOrder",
                table: "CategoryFieldOptions",
                columns: new[] { "FieldDefinitionId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFieldDefinitions_CategoryId_SortOrder",
                table: "CategoryFieldDefinitions",
                columns: new[] { "CategoryId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsActive",
                table: "Categories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SortOrder",
                table: "Categories",
                column: "SortOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Listings_CreatedAtUtc",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_Price",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_Status",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_Status_CategoryId_CreatedAtUtc",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_CategoryFieldOptions_FieldDefinitionId_SortOrder",
                table: "CategoryFieldOptions");

            migrationBuilder.DropIndex(
                name: "IX_CategoryFieldDefinitions_CategoryId_SortOrder",
                table: "CategoryFieldDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_Categories_IsActive",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SortOrder",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "ListingFavorites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingFavorites_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFieldOptions_FieldDefinitionId",
                table: "CategoryFieldOptions",
                column: "FieldDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFieldDefinitions_CategoryId",
                table: "CategoryFieldDefinitions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingFavorites_ListingId",
                table: "ListingFavorites",
                column: "ListingId");
        }
    }
}
