using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlleycatApp.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagueScoreModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeagueScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Score = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    AttendeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeagueId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeagueScores_AspNetUsers_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeagueScores_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueScores_AttendeeId",
                table: "LeagueScores",
                column: "AttendeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueScores_LeagueId",
                table: "LeagueScores",
                column: "LeagueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueScores");
        }
    }
}
