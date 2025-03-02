using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvPlaylistManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddM3UEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "M3UPlaylist",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EpgSourceId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M3UPlaylist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_M3UPlaylist_EpgSources_EpgSourceId",
                        column: x => x.EpgSourceId,
                        principalTable: "EpgSources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "M3UChannelGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    M3UPlaylistId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M3UChannelGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_M3UChannelGroup_M3UPlaylist_M3UPlaylistId",
                        column: x => x.M3UPlaylistId,
                        principalTable: "M3UPlaylist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "M3UChannel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TvgId = table.Column<string>(type: "TEXT", nullable: true),
                    TvgName = table.Column<string>(type: "TEXT", nullable: true),
                    TvgLogo = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelGroupId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M3UChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_M3UChannel_M3UChannelGroup_ChannelGroupId",
                        column: x => x.ChannelGroupId,
                        principalTable: "M3UChannelGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_M3UChannel_ChannelGroupId",
                table: "M3UChannel",
                column: "ChannelGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_M3UChannelGroup_M3UPlaylistId",
                table: "M3UChannelGroup",
                column: "M3UPlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_M3UPlaylist_EpgSourceId",
                table: "M3UPlaylist",
                column: "EpgSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_M3UPlaylist_Name",
                table: "M3UPlaylist",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_M3UPlaylist_Url",
                table: "M3UPlaylist",
                column: "Url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "M3UChannel");

            migrationBuilder.DropTable(
                name: "M3UChannelGroup");

            migrationBuilder.DropTable(
                name: "M3UPlaylist");
        }
    }
}
