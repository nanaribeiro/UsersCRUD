using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UsersCrud.Infra.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dto");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ID", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_USER_KEY",
                schema: "dto",
                table: "Users",
                columns: new[] { "UserName", "Email" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "dto");
        }
    }
}
