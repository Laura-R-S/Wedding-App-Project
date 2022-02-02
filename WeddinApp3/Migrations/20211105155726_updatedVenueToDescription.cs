using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddinApp3.Migrations
{
    public partial class updatedVenueToDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Venue",
                table: "Appointments",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Appointments",
                newName: "Venue");
        }
    }
}
