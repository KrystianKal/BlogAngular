using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAngular.Migrations
{
    /// <inheritdoc />
    public partial class FixArticleFavoritedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleFavoriteds_Articles_ArticleId1",
                table: "ArticleFavoriteds");

            migrationBuilder.DropIndex(
                name: "IX_ArticleFavoriteds_ArticleId1",
                table: "ArticleFavoriteds");

            migrationBuilder.DropColumn(
                name: "ArticleId1",
                table: "ArticleFavoriteds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArticleId1",
                table: "ArticleFavoriteds",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleFavoriteds_ArticleId1",
                table: "ArticleFavoriteds",
                column: "ArticleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleFavoriteds_Articles_ArticleId1",
                table: "ArticleFavoriteds",
                column: "ArticleId1",
                principalTable: "Articles",
                principalColumn: "ArticleId");
        }
    }
}
