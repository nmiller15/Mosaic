using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mosaic.Migrations
{
    /// <inheritdoc />
    public partial class FixLogMessagePrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LogMessages",
                table: "LogMessages");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LogMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogMessages",
                table: "LogMessages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LogMessages_SourceKey",
                table: "LogMessages",
                column: "SourceKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LogMessages",
                table: "LogMessages");

            migrationBuilder.DropIndex(
                name: "IX_LogMessages_SourceKey",
                table: "LogMessages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LogMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogMessages",
                table: "LogMessages",
                column: "SourceKey");
        }
    }
}
