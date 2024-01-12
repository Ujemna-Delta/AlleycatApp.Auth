using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlleycatApp.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRaceAttendanceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RaceAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    AttendeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaceAttendances_AspNetUsers_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceAttendances_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceAttendances_AttendeeId",
                table: "RaceAttendances",
                column: "AttendeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceAttendances_RaceId",
                table: "RaceAttendances",
                column: "RaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceAttendances");
        }
    }
}
