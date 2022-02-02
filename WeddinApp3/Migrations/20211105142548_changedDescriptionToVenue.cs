using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddinApp3.Migrations
{
    public partial class changedDescriptionToVenue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "supplierApprovedApt",
                table: "Appointments",
                newName: "SupplierApprovedApt");

            migrationBuilder.RenameColumn(
                name: "adminId",
                table: "Appointments",
                newName: "AdminId");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Appointments",
                newName: "Venue");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Appointments",
                newName: "Duration");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Appointments",
                newName: "SupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupplierApprovedApt",
                table: "Appointments",
                newName: "supplierApprovedApt");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Appointments",
                newName: "adminId");

            migrationBuilder.RenameColumn(
                name: "Venue",
                table: "Appointments",
                newName: "SuuplierId");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Appointments",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Appointments",
                newName: "Durtaion");
        }
    }
}
