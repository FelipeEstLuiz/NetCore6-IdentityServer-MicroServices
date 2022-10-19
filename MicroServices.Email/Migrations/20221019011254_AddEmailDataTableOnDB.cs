using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroServices.Email.Migrations
{
    public partial class AddEmailDataTableOnDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "email_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    log = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false),
                    sent_date = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValue: new DateTime(2022, 10, 19, 1, 12, 54, 151, DateTimeKind.Utc).AddTicks(2497))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_logs", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "email_logs");
        }
    }
}
