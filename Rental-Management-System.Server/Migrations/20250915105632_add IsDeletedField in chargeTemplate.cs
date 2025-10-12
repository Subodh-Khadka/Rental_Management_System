using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental_Management_System.Server.Migrations
{
    /// <inheritdoc />
    public partial class addIsDeletedFieldinchargeTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ChargeTemplates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChargeTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ChargeTemplates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChargeTemplates");
        }
    }
}
