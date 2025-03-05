using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Packages.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addusercolumnforfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UploadedFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFiles_UserId",
                table: "UploadedFiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadedFiles_Users_UserId",
                table: "UploadedFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadedFiles_Users_UserId",
                table: "UploadedFiles");

            migrationBuilder.DropIndex(
                name: "IX_UploadedFiles_UserId",
                table: "UploadedFiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UploadedFiles");
        }
    }
}
