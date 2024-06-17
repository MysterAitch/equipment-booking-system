﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquipmentBookingSystem.Website.Migrations;

/// <inheritdoc />
public partial class AddBasicEvent : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Events",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EventCoverStart = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                EventCoverEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                EventStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                EventEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                DipsEventLocationString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DipsEventPostcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DipsEventType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DipsIsDeleted = table.Column<bool>(type: "bit", nullable: false),
                DipsId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DipsEventTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DipsAllocatedArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DipsAllocatedDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Events", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Events");
    }
}