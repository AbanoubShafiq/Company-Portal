using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCalculationLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtpVerifications_Companies_CompanyId",
                table: "OtpVerifications");

            migrationBuilder.DropIndex(
                name: "IX_OtpVerifications_CompanyId",
                table: "OtpVerifications");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "OtpVerifications");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "Companies");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordSalt",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "OtpVerifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordSalt",
                table: "Companies",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Companies",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Companies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerifications_CompanyId",
                table: "OtpVerifications",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_OtpVerifications_Companies_CompanyId",
                table: "OtpVerifications",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
