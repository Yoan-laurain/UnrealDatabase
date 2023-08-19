using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalApi.Migrations
{
    /// <inheritdoc />
    public partial class JWT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LoginAttempts",
                columns: table => new
                {
                    LoginAttemptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoginResultId = table.Column<short>(type: "smallint", nullable: false),
                    WinOsSerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinOsUserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAttempts", x => x.LoginAttemptId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginAttempts");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
