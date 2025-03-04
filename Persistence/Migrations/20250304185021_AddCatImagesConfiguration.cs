using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCatImagesConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cats_CatImage_ImageId",
                table: "Cats");

            migrationBuilder.DropIndex(
                name: "IX_Cats_ImageId",
                table: "Cats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatImage",
                table: "CatImage");

            migrationBuilder.RenameTable(
                name: "CatImage",
                newName: "CatImages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatImages",
                table: "CatImages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CatImages_CatId",
                table: "CatImages",
                column: "CatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CatImages_Cats_CatId",
                table: "CatImages",
                column: "CatId",
                principalTable: "Cats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatImages_Cats_CatId",
                table: "CatImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatImages",
                table: "CatImages");

            migrationBuilder.DropIndex(
                name: "IX_CatImages_CatId",
                table: "CatImages");

            migrationBuilder.RenameTable(
                name: "CatImages",
                newName: "CatImage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatImage",
                table: "CatImage",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Cats_ImageId",
                table: "Cats",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Cats_CatImage_ImageId",
                table: "Cats",
                column: "ImageId",
                principalTable: "CatImage",
                principalColumn: "Id");
        }
    }
}
