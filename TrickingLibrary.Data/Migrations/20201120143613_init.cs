using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TrickingLibrary.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Difficulties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Difficulties_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModerationItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: false),
                    Current = table.Column<int>(nullable: false),
                    Target = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Rejected = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModerationItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModerationItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    TrickId = table.Column<string>(nullable: true),
                    VideoProcessed = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tricks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tricks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tricks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: false),
                    ModerationItemId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_ModerationItems_ModerationItemId",
                        column: x => x.ModerationItemId,
                        principalTable: "ModerationItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    HtmlContent = table.Column<string>(nullable: true),
                    ModerationItemId = table.Column<int>(nullable: true),
                    SubmissionId = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_ModerationItems_ModerationItemId",
                        column: x => x.ModerationItemId,
                        principalTable: "ModerationItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubmissionVotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: false),
                    SubmissionId = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmissionVotes_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmissionVotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    SubmissionId = table.Column<int>(nullable: true),
                    VideoLink = table.Column<string>(nullable: true),
                    ThumbLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrickCategories",
                columns: table => new
                {
                    TrickId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrickCategories", x => new { x.CategoryId, x.TrickId });
                    table.ForeignKey(
                        name: "FK_TrickCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrickCategories_Tricks_TrickId",
                        column: x => x.TrickId,
                        principalTable: "Tricks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrickDifficulties",
                columns: table => new
                {
                    TrickId = table.Column<int>(nullable: false),
                    DifficultyId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrickDifficulties", x => new { x.DifficultyId, x.TrickId });
                    table.ForeignKey(
                        name: "FK_TrickDifficulties_Difficulties_DifficultyId",
                        column: x => x.DifficultyId,
                        principalTable: "Difficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrickDifficulties_Tricks_TrickId",
                        column: x => x.TrickId,
                        principalTable: "Tricks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrickRelationships",
                columns: table => new
                {
                    PrerequisiteId = table.Column<int>(nullable: false),
                    ProgressionId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrickRelationships", x => new { x.PrerequisiteId, x.ProgressionId });
                    table.ForeignKey(
                        name: "FK_TrickRelationships_Tricks_PrerequisiteId",
                        column: x => x.PrerequisiteId,
                        principalTable: "Tricks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrickRelationships_Tricks_ProgressionId",
                        column: x => x.ProgressionId,
                        principalTable: "Tricks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ModerationItemId",
                table: "Comments",
                column: "ModerationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SubmissionId",
                table: "Comments",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Difficulties_UserId",
                table: "Difficulties",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationItems_UserId",
                table: "ModerationItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ModerationItemId",
                table: "Reviews",
                column: "ModerationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_UserId",
                table: "Submissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionVotes_SubmissionId",
                table: "SubmissionVotes",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionVotes_UserId",
                table: "SubmissionVotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrickCategories_TrickId",
                table: "TrickCategories",
                column: "TrickId");

            migrationBuilder.CreateIndex(
                name: "IX_TrickDifficulties_TrickId",
                table: "TrickDifficulties",
                column: "TrickId");

            migrationBuilder.CreateIndex(
                name: "IX_TrickRelationships_ProgressionId",
                table: "TrickRelationships",
                column: "ProgressionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tricks_UserId",
                table: "Tricks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_SubmissionId",
                table: "Videos",
                column: "SubmissionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "SubmissionVotes");

            migrationBuilder.DropTable(
                name: "TrickCategories");

            migrationBuilder.DropTable(
                name: "TrickDifficulties");

            migrationBuilder.DropTable(
                name: "TrickRelationships");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "ModerationItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Difficulties");

            migrationBuilder.DropTable(
                name: "Tricks");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
