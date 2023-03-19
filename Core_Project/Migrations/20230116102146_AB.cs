using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core_Project.Migrations
{
    public partial class AB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CF",
                columns: table => new
                {
                    STUDENT_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BATCH_ID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CF", x => x.STUDENT_ID);
                });

            migrationBuilder.CreateTable(
                name: "IDB_COURS",
                columns: table => new
                {
                    SUB_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDB_SUBJECT = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDB_COURS", x => x.SUB_ID);
                });

            migrationBuilder.CreateTable(
                name: "CORE_STUDENTS",
                columns: table => new
                {
                    SL_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STUDENT_ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SUB_ID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CORE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PICTURE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORE_STUDENTS", x => x.SL_NO);
                    table.ForeignKey(
                        name: "FK_CORE_STUDENTS_CF_STUDENT_ID",
                        column: x => x.STUDENT_ID,
                        principalTable: "CF",
                        principalColumn: "STUDENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CORE_STUDENTS_IDB_COURS_SUB_ID",
                        column: x => x.SUB_ID,
                        principalTable: "IDB_COURS",
                        principalColumn: "SUB_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CORE_STUDENTS_STUDENT_ID",
                table: "CORE_STUDENTS",
                column: "STUDENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CORE_STUDENTS_SUB_ID",
                table: "CORE_STUDENTS",
                column: "SUB_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CORE_STUDENTS");

            migrationBuilder.DropTable(
                name: "CF");

            migrationBuilder.DropTable(
                name: "IDB_COURS");
        }
    }
}
