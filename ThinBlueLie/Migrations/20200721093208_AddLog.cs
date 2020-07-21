using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLibrary.Migrations
{
    public partial class AddLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    IdLog = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeStamp = table.Column<string>(nullable: true),
                    Action = table.Column<int>(nullable: false),
                    IpAddress = table.Column<string>(nullable: true),
                    IdUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.IdLog);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "SourceFrom",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "FlagType",
                table: "Flagged");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Flagged");

            migrationBuilder.AlterColumn<int>(
                name: "SourceFile",
                table: "Media",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceLocation",
                table: "Media",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Media",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Logins",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Logins",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Flagged",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Flagged",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
