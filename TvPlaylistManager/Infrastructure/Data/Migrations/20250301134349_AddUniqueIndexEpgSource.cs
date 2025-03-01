using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvPlaylistManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexEpgSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EpgSources_Alias",
                table: "EpgSources");

            migrationBuilder.CreateIndex(
                name: "IX_EpgSources_Alias",
                table: "EpgSources",
                column: "Alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EpgSources_Url",
                table: "EpgSources",
                column: "Url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EpgSources_Alias",
                table: "EpgSources");

            migrationBuilder.DropIndex(
                name: "IX_EpgSources_Url",
                table: "EpgSources");

            migrationBuilder.CreateIndex(
                name: "IX_EpgSources_Alias",
                table: "EpgSources",
                column: "Alias");
        }
    }
}
