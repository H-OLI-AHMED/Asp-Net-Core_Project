using Microsoft.EntityFrameworkCore.Migrations;

namespace Core_Project.Migrations
{
    public partial class aaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IDB_SUBJECT",
                table: "CORE_STUDENTS",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IDB_SUBJECT",
                table: "CORE_STUDENTS");
        }
    }
}
