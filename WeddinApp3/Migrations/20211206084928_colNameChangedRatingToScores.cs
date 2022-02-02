using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddinApp3.Migrations
{
    public partial class colNameChangedRatingToScores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Ratings",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Ratings",
                table: "Ratings",
                newName: "Scores");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Ratings",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Scores",
                table: "Ratings",
                newName: "Ratings");
        }
    }
}
