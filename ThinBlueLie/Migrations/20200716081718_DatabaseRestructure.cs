using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLibrary.Migrations
{
    public partial class DatabaseRestructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "timelineinfo");

            migrationBuilder.DropColumn(
                name: "Gore",
                table: "timelineinfo");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "timelineinfo");

            migrationBuilder.DropColumn(
                name: "VidLink",
                table: "timelineinfo");

            migrationBuilder.AlterColumn<sbyte>(
                name: "Weapon",
                table: "timelineinfo",
                type: "TINYINT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldNullable: true);

            migrationBuilder.AlterColumn<sbyte>(
                name: "SubjectSex",
                table: "timelineinfo",
                type: "TINYINT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)");

            migrationBuilder.AlterColumn<sbyte>(
                name: "SubjectRace",
                table: "timelineinfo",
                type: "TINYINT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectName",
                table: "timelineinfo",
                type: "varchar(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<sbyte>(
                name: "State",
                table: "timelineinfo",
                type: "TINYINT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)");

            migrationBuilder.AlterColumn<sbyte>(
                name: "OfficerSex",
                table: "timelineinfo",
                type: "TINYINT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)");

            migrationBuilder.AlterColumn<sbyte>(
                name: "OfficerRace",
                table: "timelineinfo",
                type: "TINYINT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)");

            migrationBuilder.AlterColumn<string>(
                name: "OfficerName",
                table: "timelineinfo",
                type: "varchar(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<sbyte>(
                name: "Misconduct",
                table: "timelineinfo",
                type: "TINYINT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "timelineinfo",
                type: "CHAR(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)");

            migrationBuilder.AlterColumn<string>(
                name: "Context",
                table: "timelineinfo",
                type: "MEDIUMTEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "timelineinfo",
                type: "varchar(20)",
                maxLength: 86,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 86);

            migrationBuilder.AlterColumn<sbyte>(
                name: "Armed",
                table: "timelineinfo",
                type: "TINYINT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)");

            migrationBuilder.AddColumn<sbyte>(
                name: "Locked",
                table: "timelineinfo",
                type: "TINYINT",
                nullable: false,
                defaultValue: (sbyte)0);

            migrationBuilder.AddColumn<string>(
                name: "SubmittedBy",
                table: "timelineinfo",
                type: "VARCHAR(50)",
                nullable: true);

            migrationBuilder.AddColumn<sbyte>(
                name: "Verified",
                table: "timelineinfo",
                type: "TINYINT",
                nullable: false,
                defaultValue: (sbyte)0);

            migrationBuilder.AlterColumn<bool>(
                name: "TwoFactorEnabled",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(1)");

            migrationBuilder.CreateTable(
                name: "Flagged",
                columns: table => new
                {
                    IdFlagged = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTimelineInfo = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flagged", x => x.IdFlagged);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    IdLogin = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.IdLogin);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    IdMedia = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    IdTimelineinfo = table.Column<int>(nullable: false),
                    SourceLocation = table.Column<string>(nullable: true),
                    Gore = table.Column<int>(nullable: false),
                    SourceFile = table.Column<int>(nullable: false),
                    Blurb = table.Column<string>(nullable: true),
                    SubmittedBy = table.Column<string>(type: "VARCHAR(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.IdMedia);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flagged");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropColumn(
                name: "Locked",
                table: "timelineinfo");

            migrationBuilder.DropColumn(
                name: "SubmittedBy",
                table: "timelineinfo");

            migrationBuilder.DropColumn(
                name: "Verified",
                table: "timelineinfo");

            migrationBuilder.AlterColumn<string>(
                name: "Weapon",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: true,
                oldClrType: typeof(sbyte),
                oldType: "TINYINT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectSex",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "TINYINT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectRace",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "TINYINT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectName",
                table: "timelineinfo",
                type: "varchar(45)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(60)",
                oldMaxLength: 60,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "TINYINT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "OfficerSex",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "TINYINT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "OfficerRace",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "TINYINT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "OfficerName",
                table: "timelineinfo",
                type: "varchar(45)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(60)",
                oldMaxLength: 60,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Misconduct",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: true,
                oldClrType: typeof(sbyte),
                oldType: "TINYINT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "CHAR(10)",
                oldMaxLength: 10)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Context",
                table: "timelineinfo",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "MEDIUMTEXT",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "timelineinfo",
                type: "varchar(45)",
                maxLength: 86,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 86)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Armed",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "TINYINT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AddColumn<string>(
                name: "Credit",
                table: "timelineinfo",
                type: "VARCHAR(60)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gore",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AddColumn<sbyte>(
                name: "Source",
                table: "timelineinfo",
                type: "TINYINT(1)",
                nullable: false,
                defaultValue: (sbyte)0);

            migrationBuilder.AddColumn<string>(
                name: "VidLink",
                table: "timelineinfo",
                type: "varchar(45)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<sbyte>(
                name: "TwoFactorEnabled",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<sbyte>(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<sbyte>(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<sbyte>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
