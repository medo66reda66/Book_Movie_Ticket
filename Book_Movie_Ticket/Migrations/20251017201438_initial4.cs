using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Movie_Ticket.Migrations
{
    /// <inheritdoc />
    public partial class initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieSupimg_Movies_MovieId",
                table: "MovieSupimg");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieSupimg",
                table: "MovieSupimg");

            migrationBuilder.RenameTable(
                name: "MovieSupimg",
                newName: "MovieSupimgs");

            migrationBuilder.RenameIndex(
                name: "IX_MovieSupimg_MovieId",
                table: "MovieSupimgs",
                newName: "IX_MovieSupimgs_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieSupimgs",
                table: "MovieSupimgs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSupimgs_Movies_MovieId",
                table: "MovieSupimgs",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieSupimgs_Movies_MovieId",
                table: "MovieSupimgs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieSupimgs",
                table: "MovieSupimgs");

            migrationBuilder.RenameTable(
                name: "MovieSupimgs",
                newName: "MovieSupimg");

            migrationBuilder.RenameIndex(
                name: "IX_MovieSupimgs_MovieId",
                table: "MovieSupimg",
                newName: "IX_MovieSupimg_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieSupimg",
                table: "MovieSupimg",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSupimg_Movies_MovieId",
                table: "MovieSupimg",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
