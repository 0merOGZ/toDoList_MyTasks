using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tod.Migrations
{
    /// <inheritdoc />
    public partial class ArchiveSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArchivedDate",
                table: "Todos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Todos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArchivedDate",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Todos");
        }
    }
}
