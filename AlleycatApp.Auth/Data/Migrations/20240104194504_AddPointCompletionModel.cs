using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlleycatApp.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPointCompletionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointCompletions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PointId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointCompletions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointCompletions_AspNetUsers_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointCompletions_Points_PointId",
                        column: x => x.PointId,
                        principalTable: "Points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointCompletions_AttendeeId",
                table: "PointCompletions",
                column: "AttendeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PointCompletions_PointId",
                table: "PointCompletions",
                column: "PointId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointCompletions");
        }
    }
}
