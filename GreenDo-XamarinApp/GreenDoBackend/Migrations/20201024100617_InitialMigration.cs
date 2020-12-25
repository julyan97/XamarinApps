using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenDoBackend.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LevelUpConfig",
                columns: table => new
                {
                    Level = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequiredExperience = table.Column<int>(nullable: false),
                    MaxRandomCare = table.Column<int>(nullable: false),
                    MaxRandomHeart = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelUpConfig", x => x.Level);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Type = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "LevelStats",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Experience = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelStats", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_LevelStats_LevelUpConfig_Level",
                        column: x => x.Level,
                        principalTable: "LevelUpConfig",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    LevelStatsId = table.Column<Guid>(nullable: false),
                    ReactionResourcesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_LevelStats_LevelStatsId",
                        column: x => x.LevelStatsId,
                        principalTable: "LevelStats",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReactionResources",
                columns: table => new
                {
                    UserReactionResourceId = table.Column<Guid>(nullable: false),
                    Available = table.Column<int>(nullable: false),
                    OfUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionResources", x => x.UserReactionResourceId);
                    table.ForeignKey(
                        name: "FK_ReactionResources_Users_OfUserId",
                        column: x => x.OfUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReactionVideo",
                columns: table => new
                {
                    ReactionId = table.Column<string>(nullable: false),
                    VideoId = table.Column<Guid>(nullable: false),
                    ReacterId = table.Column<Guid>(nullable: false),
                    ReactedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionVideo", x => new { x.ReactionId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_ReactionVideo_Users_ReacterId",
                        column: x => x.ReacterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReactionVideo_Reactions_ReactionId",
                        column: x => x.ReactionId,
                        principalTable: "Reactions",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReactionVideo_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LevelStats_Level",
                table: "LevelStats",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionResources_OfUserId",
                table: "ReactionResources",
                column: "OfUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReactionVideo_ReacterId",
                table: "ReactionVideo",
                column: "ReacterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionVideo_VideoId",
                table: "ReactionVideo",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LevelStatsId",
                table: "Users",
                column: "LevelStatsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_CreatedById",
                table: "Videos",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReactionResources");

            migrationBuilder.DropTable(
                name: "ReactionVideo");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LevelStats");

            migrationBuilder.DropTable(
                name: "LevelUpConfig");
        }
    }
}
