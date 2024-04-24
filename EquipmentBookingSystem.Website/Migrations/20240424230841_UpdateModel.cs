﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquipmentBookingSystem.Website.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "Item",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Item",
                newName: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Item",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Item",
                newName: "CreatedAt");
        }
    }
}
