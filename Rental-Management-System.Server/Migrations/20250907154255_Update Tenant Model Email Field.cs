using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental_Management_System.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTenantModelEmailField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailAdress",
                table: "Tenants",
                newName: "EmailAddress");

            migrationBuilder.AlterColumn<string>(
                name: "ChargeType",
                table: "MonthlyCharges",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Tenants",
                newName: "EmailAdress");

            migrationBuilder.AlterColumn<string>(
                name: "ChargeType",
                table: "MonthlyCharges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
