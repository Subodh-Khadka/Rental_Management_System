using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental_Management_System.Server.Migrations
{
    /// <inheritdoc />
    public partial class chargeTemplatemodelmodifymonthlyCharge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChargeTemplateId",
                table: "MonthlyCharges",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ChargeTemplates",
                columns: table => new
                {
                    ChargeTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChargeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsVariable = table.Column<bool>(type: "bit", nullable: false),
                    CalculationMethod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeTemplates", x => x.ChargeTemplateId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyCharges_ChargeTemplateId",
                table: "MonthlyCharges",
                column: "ChargeTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyCharges_ChargeTemplates_ChargeTemplateId",
                table: "MonthlyCharges",
                column: "ChargeTemplateId",
                principalTable: "ChargeTemplates",
                principalColumn: "ChargeTemplateId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyCharges_ChargeTemplates_ChargeTemplateId",
                table: "MonthlyCharges");

            migrationBuilder.DropTable(
                name: "ChargeTemplates");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyCharges_ChargeTemplateId",
                table: "MonthlyCharges");

            migrationBuilder.DropColumn(
                name: "ChargeTemplateId",
                table: "MonthlyCharges");
        }
    }
}
