using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquipmentBookingSystem.Website.Migrations;

/// <inheritdoc />
public partial class AddItemNotes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "DamageNotes",
            table: "Item",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Notes",
            table: "Item",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DamageNotes",
            table: "Item");

        migrationBuilder.DropColumn(
            name: "Notes",
            table: "Item");
    }
}
