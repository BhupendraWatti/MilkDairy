﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MIlkDairy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addForeginKeyrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyID",
                table: "AspNetUsers",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Company_CompanyID",
                table: "AspNetUsers",
                column: "CompanyID",
                principalTable: "Company",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Company_CompanyID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "AspNetUsers");
        }
    }
}
