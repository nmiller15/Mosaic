using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mosaic.Migrations
{
    /// <inheritdoc />
    public partial class AddAppSourceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSources", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppSources",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[] { 1, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Investigations app source", "Investigations" });

            migrationBuilder.AddForeignKey(
                name: "FK_LogMessages_AppSources_SourceKey",
                table: "LogMessages",
                column: "SourceKey",
                principalTable: "AppSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogMessages_AppSources_SourceKey",
                table: "LogMessages");

            migrationBuilder.DropTable(
                name: "AppSources");
        }
    }
}
