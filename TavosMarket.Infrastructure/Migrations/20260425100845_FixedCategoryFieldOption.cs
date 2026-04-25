using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavosMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedCategoryFieldOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryFieldOption_CategoryFieldDefinitions_FieldDefinitionId",
                table: "CategoryFieldOption");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingFieldValues_CategoryFieldOption_OptionId",
                table: "ListingFieldValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryFieldOption",
                table: "CategoryFieldOption");

            migrationBuilder.RenameTable(
                name: "CategoryFieldOption",
                newName: "CategoryFieldOptions");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryFieldOption_FieldDefinitionId",
                table: "CategoryFieldOptions",
                newName: "IX_CategoryFieldOptions_FieldDefinitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryFieldOptions",
                table: "CategoryFieldOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryFieldOptions_CategoryFieldDefinitions_FieldDefinitionId",
                table: "CategoryFieldOptions",
                column: "FieldDefinitionId",
                principalTable: "CategoryFieldDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingFieldValues_CategoryFieldOptions_OptionId",
                table: "ListingFieldValues",
                column: "OptionId",
                principalTable: "CategoryFieldOptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryFieldOptions_CategoryFieldDefinitions_FieldDefinitionId",
                table: "CategoryFieldOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingFieldValues_CategoryFieldOptions_OptionId",
                table: "ListingFieldValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryFieldOptions",
                table: "CategoryFieldOptions");

            migrationBuilder.RenameTable(
                name: "CategoryFieldOptions",
                newName: "CategoryFieldOption");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryFieldOptions_FieldDefinitionId",
                table: "CategoryFieldOption",
                newName: "IX_CategoryFieldOption_FieldDefinitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryFieldOption",
                table: "CategoryFieldOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryFieldOption_CategoryFieldDefinitions_FieldDefinitionId",
                table: "CategoryFieldOption",
                column: "FieldDefinitionId",
                principalTable: "CategoryFieldDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingFieldValues_CategoryFieldOption_OptionId",
                table: "ListingFieldValues",
                column: "OptionId",
                principalTable: "CategoryFieldOption",
                principalColumn: "Id");
        }
    }
}
