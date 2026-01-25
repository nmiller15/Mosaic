using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mosaic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogMessages_AppSources_SourceKey",
                table: "LogMessages");

            migrationBuilder.DropIndex(
                name: "IX_LogMessages_SourceKey",
                table: "LogMessages");

            migrationBuilder.DropColumn(
                name: "Environment",
                table: "LogMessages");

            migrationBuilder.DropColumn(
                name: "SourceKey",
                table: "LogMessages");

            migrationBuilder.DropColumn(
                name: "StackTrace",
                table: "LogMessages");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "LogMessages",
                newName: "RenderedMessage");

            migrationBuilder.AlterColumn<string>(
                name: "Exception",
                table: "LogMessages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageTemplate",
                table: "LogMessages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "LogMessages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageTemplate",
                table: "LogMessages");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "LogMessages");

            migrationBuilder.RenameColumn(
                name: "RenderedMessage",
                table: "LogMessages",
                newName: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "Exception",
                table: "LogMessages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Environment",
                table: "LogMessages",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SourceKey",
                table: "LogMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StackTrace",
                table: "LogMessages",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogMessages_SourceKey",
                table: "LogMessages",
                column: "SourceKey");

            migrationBuilder.AddForeignKey(
                name: "FK_LogMessages_AppSources_SourceKey",
                table: "LogMessages",
                column: "SourceKey",
                principalTable: "AppSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
