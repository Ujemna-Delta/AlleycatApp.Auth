﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlleycatApp.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRaceScoreType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "RaceCompletions",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "RaceCompletions",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");
        }
    }
}
