using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlleycatApp.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRaceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BeginTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ValueModifier = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFreeOrder = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Races");
        }
    }
}
