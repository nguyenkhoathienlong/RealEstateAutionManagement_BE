using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class updateauctiondesc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Auctions");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RealEstates",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "RealEstates");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Auctions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
