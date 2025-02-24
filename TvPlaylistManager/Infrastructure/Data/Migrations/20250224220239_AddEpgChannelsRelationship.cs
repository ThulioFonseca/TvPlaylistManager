using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvPlaylistManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEpgChannelsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EpgChannel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelEpgId = table.Column<string>(type: "TEXT", nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Keywords = table.Column<string>(type: "TEXT", nullable: true),
                    EpgSourceId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpgChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpgChannel_EpgSources_EpgSourceId",
                        column: x => x.EpgSourceId,
                        principalTable: "EpgSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpgSources_Alias",
                table: "EpgSources",
                column: "Alias");

            migrationBuilder.CreateIndex(
                name: "IX_EpgChannel_ChannelEpgId",
                table: "EpgChannel",
                column: "ChannelEpgId");

            migrationBuilder.CreateIndex(
                name: "IX_EpgChannel_EpgSourceId",
                table: "EpgChannel",
                column: "EpgSourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpgChannel");

            migrationBuilder.DropIndex(
                name: "IX_EpgSources_Alias",
                table: "EpgSources");
        }
    }
}
