using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class updatetransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_UserBids_UserBidId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "UserBidId",
                table: "Transaction",
                newName: "AuctionId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_UserBidId",
                table: "Transaction",
                newName: "IX_Transaction_AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Auctions_AuctionId",
                table: "Transaction",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Auctions_AuctionId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "AuctionId",
                table: "Transaction",
                newName: "UserBidId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_AuctionId",
                table: "Transaction",
                newName: "IX_Transaction_UserBidId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_UserBids_UserBidId",
                table: "Transaction",
                column: "UserBidId",
                principalTable: "UserBids",
                principalColumn: "Id");
        }
    }
}
