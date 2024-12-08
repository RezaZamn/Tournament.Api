using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_TournamentDetails_TournamentDetailsId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_TournamentDetailsId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "TournamentDetailsId",
                table: "Game");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TournamentDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Game_TournamentId",
                table: "Game",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_TournamentDetails_TournamentId",
                table: "Game",
                column: "TournamentId",
                principalTable: "TournamentDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_TournamentDetails_TournamentId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_TournamentId",
                table: "Game");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TournamentDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "TournamentDetailsId",
                table: "Game",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_TournamentDetailsId",
                table: "Game",
                column: "TournamentDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_TournamentDetails_TournamentDetailsId",
                table: "Game",
                column: "TournamentDetailsId",
                principalTable: "TournamentDetails",
                principalColumn: "Id");
        }
    }
}
