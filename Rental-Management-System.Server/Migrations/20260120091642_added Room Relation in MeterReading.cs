using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental_Management_System.Server.Migrations
{
    /// <inheritdoc />
    public partial class addedRoomRelationinMeterReading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterReadings_RentPayments_RentPaymentPaymentId",
                table: "MeterReadings");

            migrationBuilder.AlterColumn<Guid>(
                name: "RentPaymentPaymentId",
                table: "MeterReadings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Month",
                table: "MeterReadings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "MeterReadings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadings_RoomId",
                table: "MeterReadings",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterReadings_RentPayments_RentPaymentPaymentId",
                table: "MeterReadings",
                column: "RentPaymentPaymentId",
                principalTable: "RentPayments",
                principalColumn: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterReadings_Rooms_RoomId",
                table: "MeterReadings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterReadings_RentPayments_RentPaymentPaymentId",
                table: "MeterReadings");

            migrationBuilder.DropForeignKey(
                name: "FK_MeterReadings_Rooms_RoomId",
                table: "MeterReadings");

            migrationBuilder.DropIndex(
                name: "IX_MeterReadings_RoomId",
                table: "MeterReadings");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "MeterReadings");

            migrationBuilder.AlterColumn<Guid>(
                name: "RentPaymentPaymentId",
                table: "MeterReadings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Month",
                table: "MeterReadings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterReadings_RentPayments_RentPaymentPaymentId",
                table: "MeterReadings",
                column: "RentPaymentPaymentId",
                principalTable: "RentPayments",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
