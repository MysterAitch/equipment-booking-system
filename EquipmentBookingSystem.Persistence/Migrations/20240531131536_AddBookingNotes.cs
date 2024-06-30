using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquipmentBookingSystem.Website.Migrations;

/// <inheritdoc />
public partial class AddBookingNotes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "SjaEventDipsId",
            table: "Booking",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "SjaEventName",
            table: "Booking",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "SjaEventType",
            table: "Booking",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "SjaEventDipsId",
            table: "Booking");

        migrationBuilder.DropColumn(
            name: "SjaEventName",
            table: "Booking");

        migrationBuilder.DropColumn(
            name: "SjaEventType",
            table: "Booking");
    }
}
