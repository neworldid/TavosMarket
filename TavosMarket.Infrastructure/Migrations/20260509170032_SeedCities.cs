using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TavosMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedCities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("01b4b15d-dc76-c115-de5d-18b3ceb9a55e"), "Preiļi" },
                    { new Guid("189a7500-008c-31cb-b62d-e65d35719fa0"), "Bauska" },
                    { new Guid("2e280462-484b-f8ce-7bbc-f25eb8212ba8"), "Rīga" },
                    { new Guid("330bf69e-d9e2-9ae1-bbdd-1f630d0b2e09"), "Mārupe" },
                    { new Guid("3bfc07a8-b77c-14d4-ce00-e3c29512d9ef"), "Kuldīga" },
                    { new Guid("45ee5075-c963-94d0-56f2-4cf414e4cf3e"), "Salacgrīva" },
                    { new Guid("482e19ab-683f-04b2-ab93-60f106543dbe"), "Talsi" },
                    { new Guid("4b5d94df-1a73-daac-f022-4e33aeff0851"), "Gulbene" },
                    { new Guid("501bd21e-28ec-2048-6626-bdd0006f5b62"), "Madona" },
                    { new Guid("545c81ac-aed1-88ae-de43-b84855a60f53"), "Salaspils" },
                    { new Guid("5a66713a-c22b-b2af-9227-2ac1d6a5813d"), "Ludza" },
                    { new Guid("61365094-9744-94a9-3f3a-92930ecbaead"), "Krāslava" },
                    { new Guid("71b71022-8bf2-851c-4b45-f5efa3072bdf"), "Jēkabpils" },
                    { new Guid("7768bcc2-2d9f-c5d2-642b-462f3be3fd67"), "Liepāja" },
                    { new Guid("8410769d-da09-4f75-5029-f6a81b9092f0"), "Dobele" },
                    { new Guid("9434f1c1-237b-9a36-49b3-f496183d30cf"), "Ogre" },
                    { new Guid("98084d45-fc49-72bb-4c79-ea2dcbef75b2"), "Jelgava" },
                    { new Guid("a98e40cd-74cb-df29-9ae4-373da8201309"), "Aizkraukle" },
                    { new Guid("ade87faf-7a7f-77ab-75a4-5ca8b7ecde59"), "Cēsis" },
                    { new Guid("b24fa16a-a569-db49-7b2f-a685db019d7c"), "Balvi" },
                    { new Guid("c4cb8d95-f193-5f55-1a24-ddb63d60478d"), "Limbaži" },
                    { new Guid("ceb7ab7a-08fa-9287-6483-a4b9514f9fdb"), "Olaine" },
                    { new Guid("d9220cca-5393-b20e-0897-f5a0c8d3b51b"), "Sigulda" },
                    { new Guid("ddda60e4-7167-734c-9e7a-1a1b3d148f10"), "Valmiera" },
                    { new Guid("e64f329d-0c67-66a5-bded-606b3c46e9cf"), "Alūksne" },
                    { new Guid("e93e168f-8039-1438-c3a6-0fee97dfa487"), "Ventspils" },
                    { new Guid("f74e365d-79e1-9d58-f9e8-b3302b3e610e"), "Jūrmala" },
                    { new Guid("f8dff3b2-ec7d-5138-1bcb-0fa162b88eaf"), "Tukums" },
                    { new Guid("fc09d03f-8232-b04b-272f-132898d00293"), "Daugavpils" },
                    { new Guid("ffd00aa7-a43e-8b7d-a254-da76fcea0de0"), "Rēzekne" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("01b4b15d-dc76-c115-de5d-18b3ceb9a55e"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("189a7500-008c-31cb-b62d-e65d35719fa0"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("2e280462-484b-f8ce-7bbc-f25eb8212ba8"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("330bf69e-d9e2-9ae1-bbdd-1f630d0b2e09"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("3bfc07a8-b77c-14d4-ce00-e3c29512d9ef"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("45ee5075-c963-94d0-56f2-4cf414e4cf3e"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("482e19ab-683f-04b2-ab93-60f106543dbe"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("4b5d94df-1a73-daac-f022-4e33aeff0851"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("501bd21e-28ec-2048-6626-bdd0006f5b62"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("545c81ac-aed1-88ae-de43-b84855a60f53"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("5a66713a-c22b-b2af-9227-2ac1d6a5813d"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("61365094-9744-94a9-3f3a-92930ecbaead"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("71b71022-8bf2-851c-4b45-f5efa3072bdf"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("7768bcc2-2d9f-c5d2-642b-462f3be3fd67"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("8410769d-da09-4f75-5029-f6a81b9092f0"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("9434f1c1-237b-9a36-49b3-f496183d30cf"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("98084d45-fc49-72bb-4c79-ea2dcbef75b2"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("a98e40cd-74cb-df29-9ae4-373da8201309"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("ade87faf-7a7f-77ab-75a4-5ca8b7ecde59"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("b24fa16a-a569-db49-7b2f-a685db019d7c"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("c4cb8d95-f193-5f55-1a24-ddb63d60478d"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("ceb7ab7a-08fa-9287-6483-a4b9514f9fdb"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("d9220cca-5393-b20e-0897-f5a0c8d3b51b"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("ddda60e4-7167-734c-9e7a-1a1b3d148f10"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("e64f329d-0c67-66a5-bded-606b3c46e9cf"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("e93e168f-8039-1438-c3a6-0fee97dfa487"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("f74e365d-79e1-9d58-f9e8-b3302b3e610e"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("f8dff3b2-ec7d-5138-1bcb-0fa162b88eaf"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("fc09d03f-8232-b04b-272f-132898d00293"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("ffd00aa7-a43e-8b7d-a254-da76fcea0de0"));
        }
    }
}
