using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace newproject.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "Students",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "Students",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Emailaddress",
                table: "Students",
                newName: "EmailAddress");

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Students",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Students",
                newName: "Firstname");

            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Students",
                newName: "Emailaddress");
        }
    }
}
