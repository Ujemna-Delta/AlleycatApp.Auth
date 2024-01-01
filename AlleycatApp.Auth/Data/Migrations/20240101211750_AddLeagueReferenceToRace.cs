using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlleycatApp.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagueReferenceToRace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "LeagueId",
                table: "Races",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Races_LeagueId",
                table: "Races",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Leagues_LeagueId",
                table: "Races",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Leagues_LeagueId",
                table: "Races");

            migrationBuilder.DropIndex(
                name: "IX_Races_LeagueId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Races");
        }
    }
}
