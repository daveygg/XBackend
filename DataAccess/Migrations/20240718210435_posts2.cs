using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class posts2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaPath",
                table: "Posts2");

            migrationBuilder.AddColumn<Guid>(
                name: "MediaPath2",
                table: "Posts2",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaPath2",
                table: "Posts2");

            migrationBuilder.AddColumn<string>(
                name: "MediaPath",
                table: "Posts2",
                type: "text",
                nullable: true);
        }
    }
}
