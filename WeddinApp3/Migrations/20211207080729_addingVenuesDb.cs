using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddinApp3.Migrations
{
    public partial class addingVenuesDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VenID",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Scores",
                table: "Venues",
                type: "int",
                nullable: false,
                defaultValue: 0);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scores",
                table: "Venues");

            migrationBuilder.AlterColumn<int>(
                name: "VenID",
                table: "Venues",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

        }
    }
}
