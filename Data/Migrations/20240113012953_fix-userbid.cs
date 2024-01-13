using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class fixuserbid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BidAmount",
                table: "UserBids");

            migrationBuilder.RenameColumn(
                name: "BidNumber",
                table: "UserBids",
                newName: "Amount");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeposit",
                table: "UserBids",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeposit",
                table: "UserBids");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "UserBids",
                newName: "BidNumber");

            migrationBuilder.AddColumn<float>(
                name: "BidAmount",
                table: "UserBids",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
