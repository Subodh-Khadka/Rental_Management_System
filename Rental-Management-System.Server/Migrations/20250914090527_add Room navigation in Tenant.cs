using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental_Management_System.Server.Migrations
{
    /// <inheritdoc />
    public partial class addRoomnavigationinTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tenants_RoomId",
                table: "Tenants",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Rooms_RoomId",
                table: "Tenants",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Rooms_RoomId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_RoomId",
                table: "Tenants");
        }
    }
}
