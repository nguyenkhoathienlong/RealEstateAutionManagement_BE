using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class readdapproveby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApproveByUserId",
                table: "RealEstates",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstates_ApproveByUserId",
                table: "RealEstates",
                column: "ApproveByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstates_Users_ApproveByUserId",
                table: "RealEstates",
                column: "ApproveByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealEstates_Users_ApproveByUserId",
                table: "RealEstates");

            migrationBuilder.DropIndex(
                name: "IX_RealEstates_ApproveByUserId",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "ApproveByUserId",
                table: "RealEstates");
        }
    }
}
