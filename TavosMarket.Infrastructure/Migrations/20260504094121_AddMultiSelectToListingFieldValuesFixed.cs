using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavosMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiSelectToListingFieldValuesFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListingFieldValueSelectedOption",
                columns: table => new
                {
                    ListingFieldValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SelectedOptionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingFieldValueSelectedOption", x => new { x.ListingFieldValueId, x.SelectedOptionsId });
                    table.ForeignKey(
                        name: "FK_ListingFieldValueSelectedOption_CategoryFieldOptions_SelectedOptionsId",
                        column: x => x.SelectedOptionsId,
                        principalTable: "CategoryFieldOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListingFieldValueSelectedOption_ListingFieldValues_ListingFieldValueId",
                        column: x => x.ListingFieldValueId,
                        principalTable: "ListingFieldValues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingFieldValueSelectedOption_SelectedOptionsId",
                table: "ListingFieldValueSelectedOption",
                column: "SelectedOptionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingFieldValueSelectedOption");
        }
    }
}
