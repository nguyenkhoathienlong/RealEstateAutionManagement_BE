using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class updateauctionandsetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationStartDate",
                table: "Auctions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationEndDate",
                table: "Auctions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Auctions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "DataUnit", "DateCreate", "DateUpdate", "Description", "IsDeleted", "Key", "Value" },
                values: new object[,]
                {
                    { new Guid("d26404cc-16fe-4c3f-8fd3-775c76962119"), 0, new DateTime(2024, 1, 16, 8, 36, 54, 865, DateTimeKind.Utc).AddTicks(9647), new DateTime(2024, 1, 16, 8, 36, 54, 865, DateTimeKind.Utc).AddTicks(9649), "Phần trăm phí đăng ký.", false, "REGISTRATION_FEE_PERCENT", "0.05" },
                    { new Guid("df400b99-4408-4299-8f57-b25151d76621"), 0, new DateTime(2024, 1, 16, 8, 36, 54, 865, DateTimeKind.Utc).AddTicks(9682), new DateTime(2024, 1, 16, 8, 36, 54, 865, DateTimeKind.Utc).AddTicks(9682), "Phần trăm tiền đặt cọc.", false, "DEPOSIT_PERCENT", "10" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d26404cc-16fe-4c3f-8fd3-775c76962119"));

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("df400b99-4408-4299-8f57-b25151d76621"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Settings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationStartDate",
                table: "Auctions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationEndDate",
                table: "Auctions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Auctions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
